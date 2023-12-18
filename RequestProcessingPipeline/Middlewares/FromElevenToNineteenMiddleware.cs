namespace RequestProcessingPipeline.Middlewares
{
    public class FromElevenToNineteenMiddleware
    {
        private readonly RequestDelegate _next;

        public FromElevenToNineteenMiddleware(RequestDelegate next)
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

                // Hand Over Number To Next Middleware
                if (number % 100 < 11 || number % 100 > 19)
                {
                    await _next.Invoke(context);
                }

                else
                {
                    string[] Numbers = { "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };

                    // If Number Bigger Than 20 
                    if (number > 20)
                    {
                        Console.WriteLine("Second Middleware (11 - 19) Set To Session 'Number' :" + Numbers[number % 100 - 11]);
                        context.Session.SetString("number", Numbers[number % 100 - 11]);
                    }
                    // Number Lower Than 20
                    else
                    {
                        await context.Response.WriteAsync("Your number is " + Numbers[number % 100 - 11]);
                        Console.WriteLine("Second Middleware (11 - 19):" + Numbers[number % 100 - 11]);
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