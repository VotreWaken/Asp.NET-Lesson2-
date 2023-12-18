using RequestProcessingPipeline.Middlewares;

namespace RequestProcessingPipeline.Extensions
{
    public static class FromTwentyToHundredExtensions
    {
        public static IApplicationBuilder UseFromTwentyToHundred(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FromTwentyToHundredMiddleware>();
        }
    }
}
