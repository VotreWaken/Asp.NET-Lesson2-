namespace RequestProcessingPipeline
{
    public class FromTenThousandToFiftyThousandMiddleware
    {
        private readonly RequestDelegate _next;

        public FromTenThousandToFiftyThousandMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string? token = context.Request.Query["number"];

            try
            {
                int number = Convert.ToInt32(token);
                number = Math.Abs(number);

                if (number < 10000)
                {
                    await _next.Invoke(context);
                }
                else if (number > 50000)
                {
                    await context.Response.WriteAsync("Number greater than fifty thousand");
                }
                else
                {
                    string[] Tens = { "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
                    if (number / 10_000 == 10)
                    {
                        await context.Response.WriteAsync("Your number is one hundred thousand");
                    }
                    else if (number % 10_000 == 0)
                    {
                        await context.Response.WriteAsync("Your number is " + Tens[number / 10_000 - 2] + " thousand");
                    }
                    else
                    {
                        await _next.Invoke(context);
                        string? result = string.Empty;
                        result = context.Session.GetString("number");
                        await context.Response.WriteAsync("Your number is " + Tens[number / 10_000 - 2] + " " + result);
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
