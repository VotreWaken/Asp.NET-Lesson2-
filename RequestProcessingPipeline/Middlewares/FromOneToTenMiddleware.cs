
namespace RequestProcessingPipeline.Middlewares
{
    public class FromOneToTenMiddleware
    {
        private readonly RequestDelegate _next;

        public FromOneToTenMiddleware(RequestDelegate next)
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

                // Number Equal 10
                if (number == 10)
                {
                    await context.Response.WriteAsync("Your number is ten");
                }

                else
                {
                    string[] Ones = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

                    // If Number Bigger Than 20 
                    if (number > 20)
                    {
                        Console.WriteLine("First Middleware (1 - 10) Set To Session 'Number' :" + Ones[number % 10 - 1]);
                        context.Session.SetString("number", Ones[number % 10 - 1]);
                    }
                    // Number Lower Than 20
                    else
                    {
                        await context.Response.WriteAsync("Your number is " + Ones[number - 1]);
                        Console.WriteLine("First Middleware (1 - 10):" + Ones[number - 1]);
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