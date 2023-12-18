using RequestProcessingPipeline.Middlewares;

namespace RequestProcessingPipeline.Extensions
{
    public static class FromNineteenHundredToHundredThousandsExtensions
    {
        public static IApplicationBuilder UseFromNineteenHundredToHundredThousands(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FromNineteenHundredToHundredThousandsMiddleware>();
        }
    }
}
