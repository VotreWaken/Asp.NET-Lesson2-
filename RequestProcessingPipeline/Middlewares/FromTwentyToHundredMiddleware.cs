namespace RequestProcessingPipeline.Middlewares
{
    public class FromTwentyToHundredMiddleware
    {
        private readonly RequestDelegate _next;

        public FromTwentyToHundredMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Take number From Request
            string? token = context.Request.Query["number"];
            try
            {
                // Parse Number
                int number = Convert.ToInt32(token);
                number = Math.Abs(number);

                string? result = string.Empty;
                string[] Tens = { "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                // If Number Lower Than 20 
                if (number < 20)
                {
                    await _next.Invoke(context);
                }

                // If Number Bigger Than 100
                else if (number > 100)
                {
                    number = number % 100;
                    if (number < 20)
                    {
                        await _next.Invoke(context);
                        result = context.Session.GetString("number");
                        context.Session.SetString("number", result);
                    }
                    else if (number % 10 == 0)
                    {
                        context.Session.SetString("number", Tens[number / 10 - 2]);
                    }
                    else
                    {
                        await _next.Invoke(context);
                        result = context.Session.GetString("number");
                        context.Session.SetString("number", Tens[number / 10 - 2] + " " + result);
                    }
                }
                else
                {
                    if (number % 10 == 0)
                    {
                        await context.Response.WriteAsync("Your number is " + Tens[number / 10 - 2]);
                    }
                    else
                    {
                        await _next.Invoke(context);
                        result = context.Session.GetString("number");
                        await context.Response.WriteAsync("Your number is " + Tens[number / 10 - 2] + " " + result);
                    }
                }
            }
            catch (Exception)
            {
                await context.Response.WriteAsync("Incorrect parameter");
            }
        }
    }
}