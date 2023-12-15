namespace RequestProcessingPipeline
{
    namespace RequestProcessingPipeline
    {
        public class FromHundredToThousandMiddleware
        {
            private readonly RequestDelegate _next;

            public FromHundredToThousandMiddleware(RequestDelegate next)
            {
                this._next = next;
            }

            public async Task Invoke(HttpContext context)
            {
                string? token = context.Request.Query["number"]; // Получим число из контекста запроса
                try
                {
                    int number = Convert.ToInt32(token);
                    number = Math.Abs(number);
                    if (number < 100)
                    {
                        await _next.Invoke(context); // Передаем контекст запроса следующему компоненту
                    }
                    //else if (number > 1000)
                    //{
                    //    await context.Response.WriteAsync("Number greater than one thousand");
                    //}
                    else if (number == 1000)
                    {
                        await context.Response.WriteAsync("Your number is one thousand");
                    }
                    else
                    {
                        string[] Hundreds = { "one hundred", "two hundred", "three hundred", "four hundred", "five hundred", "six hundred", "seven hundred", "eight hundred", "nine hundred" };

                        // Если число кратно 100, например, 200, 300, ...
                        if (number % 100 == 0)
                        {
                            await context.Response.WriteAsync("Your number is " + Hundreds[number / 100 - 1]);
                        }
                        else if (number > 1000)
                        {
                            number = number % 1000;
                            if (number % 100 == 0)
                            {
                                context.Session.SetString("number", Hundreds[number / 100 - 1]);
                            }
                            else if (number / 100 != 0)
                            {
                                await _next.Invoke(context);
                                string? result = context.Session.GetString("number");
                                context.Session.SetString("number", Hundreds[number / 100 - 1] + " " + result);
                            }
                            else
                            {
                                await _next.Invoke(context);
                                string? result = context.Session.GetString("number");
                                context.Session.SetString("number", result);
                            }
                        }
                        else
                        {
                            await _next.Invoke(context); // Передаем контекст запроса следующему компоненту
                            string? result = context.Session.GetString("number");
                            Console.WriteLine(context.Request.Query["number"]);
                            // Передаем значение в сессию для следующего Middleware
                            context.Session.SetString("number", result ?? "");
                            await context.Response.WriteAsync("Your number is " + Hundreds[number / 100 - 1] + " " + (result ?? ""));
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

}
