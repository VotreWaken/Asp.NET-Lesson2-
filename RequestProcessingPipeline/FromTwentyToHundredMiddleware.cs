namespace RequestProcessingPipeline
{
    public class FromTwentyToHundredMiddleware
    {
        private readonly RequestDelegate _next;

        public FromTwentyToHundredMiddleware(RequestDelegate next)
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
                if (number < 20)
                {
                    await _next.Invoke(context); //Контекст запроса передаем следующему компоненту
                }
                //else if(number > 100)
                //{
                //    // Выдаем окончательный ответ клиенту
                //    await context.Response.WriteAsync("Number greater than one hundred");
                //}
                else if (number == 100)
                {
                    // Выдаем окончательный ответ клиенту
                    await context.Response.WriteAsync("Your number is one hundred");
                }


                else
                {
                    string[] Tens = { "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
                    if (number % 10 == 0)
                    {
                        // Выдаем окончательный ответ клиенту
                        await context.Response.WriteAsync("Your number is " + Tens[number / 10 - 2]);
                    }
                    // Если число больше 100, условие нужно для обработки больших чисел для 
                    // Правильного отображения цифр в пределах 100 + из других конвееров
                    else if (number > 100)
                    {
                        number = number % 100;
                        if (number < 20)
                        {
                            await _next.Invoke(context);
                            string? result = context.Session.GetString("number");
                            context.Session.SetString("number", result);
                        }
                        else if (number % 10 == 0)
                        {
                            context.Session.SetString("number", Tens[number / 10 - 2]);
                        }
                        else
                        {
                            await _next.Invoke(context);
                            string? result = context.Session.GetString("number");
                            context.Session.SetString("number", Tens[number / 10 - 2] + " " + result);
                        }
                    }
                    else
                    { 
                        await _next.Invoke(context); // Контекст запроса передаем следующему компоненту
                        string? result = string.Empty;
                        result = context.Session.GetString("number"); // получим число от компонента FromOneToTenMiddleware
                        // Выдаем окончательный ответ клиенту
                        await context.Response.WriteAsync("Your number is " + Tens[number / 10 - 2] + " " + result);
                    }                   
                }              
            }
            catch (Exception)
            {
                // Выдаем окончательный ответ клиенту
                await context.Response.WriteAsync("Incorrect parameter");
            }
        }
    }
}
