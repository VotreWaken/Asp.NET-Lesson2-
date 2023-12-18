using RequestProcessingPipeline.Middlewares;

namespace RequestProcessingPipeline.Extensions
{
    public static class FromElevenToNineteenExtensions
    {
        public static IApplicationBuilder UseFromElevenToNineteen(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FromElevenToNineteenMiddleware>();
        }
    }
}
