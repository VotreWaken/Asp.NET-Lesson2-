namespace RequestProcessingPipeline.Middlewares
{
    public class FromThousandToTenThousandMiddleware
    {
        private readonly RequestDelegate _next;

        public FromThousandToTenThousandMiddleware(RequestDelegate next)
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
                if (number < 1000)
                {
                    await _next.Invoke(context);
                }
                else
                {
                    string[] Thousands = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten" };
                    string? result = string.Empty;
                    if (number > 11000 && number < 20000)
                    {
                        await _next.Invoke(context);
                        result = context.Session.GetString("number");
                        context.Session.SetString("number", result);
                    }
                    else if (number > 20_000)
                    {
                        number %= 10_000;
                        if (number % 1000 == 0)
                        {
                            result = context.Session.GetString("number");
                            context.Session.SetString("number", Thousands[number / 1000 - 1] + " thousands ");
                        }
                        else if (number / 1000 == 0)
                        {
                            await _next.Invoke(context);
                            result = context.Session.GetString("number");
                            context.Session.SetString("number", "thousand " + result);
                        }
                        else
                        {
                            await _next.Invoke(context);
                            result = context.Session.GetString("number");
                            context.Session.SetString("number", Thousands[number / 1000 - 1] + " thousands " + result);
                        }
                    }
                    else
                    {
                        if (number % 1000 == 0)
                        {
                            result = context.Session.GetString("number");
                            await context.Response.WriteAsync("Your number is " + Thousands[number / 1000 - 1] + " thousand ");
                        }
                        else
                        {
                            await _next.Invoke(context);
                            result = context.Session.GetString("number");
                            await context.Response.WriteAsync("Your number is " + Thousands[number / 1000 - 1] + " thousand" + " " + result);

                        }
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
