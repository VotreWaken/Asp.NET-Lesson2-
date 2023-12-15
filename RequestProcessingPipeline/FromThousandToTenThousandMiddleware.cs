namespace RequestProcessingPipeline
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
                //else if (number > 10000)
                //{
                //    await context.Response.WriteAsync("Number greater than ten thousand");
                //}
                else
                {
                    string[] Thousands = { "one thousand", "two thousand", "three thousand", "four thousand", "five thousand", "six thousand", "seven thousand", "eight thousand", "nine thousand" };

                    if (number % 1000 == 0)
                    {
                        await context.Response.WriteAsync("Your number is " + Thousands[number / 1000 - 1]);
                    }
                    else if (number > 20_000)
                    {
                        number %= 10_000;
                        if (number % 1000 == 0)
                        {
                            string? result = context.Session.GetString("number");
                            context.Session.SetString("number", Thousands[number / 1000 - 1] + " thousands ");
                        }
                        else if (number / 1000 == 0)
                        {
                            await _next.Invoke(context);
                            string? result = context.Session.GetString("number");
                            context.Session.SetString("number", "thousand " + result);
                        }
                        else
                        {
                            await _next.Invoke(context);
                            string? result = context.Session.GetString("number");
                            context.Session.SetString("number", Thousands[number / 1000 - 1] + " thousands " + result);
                        }
                    }
                    else
                    {
                        await _next.Invoke(context);
                        string? result = context.Session.GetString("number");
                        context.Session.SetString("number", result ?? "");
                        await context.Response.WriteAsync("Your number is " + Thousands[number / 1000 - 1] + " " + (result ?? ""));
                    }
                }
            }
            catch (Exception)
            {
                await context.Response.WriteAsync("Incorrect parameter HERE");
            }
        }
    }
}
