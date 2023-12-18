using RequestProcessingPipeline.Middlewares;

namespace RequestProcessingPipeline.Extensions
{
    public static class FromOneToTenExtensions
    {
        public static IApplicationBuilder UseFromOneToTen(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FromOneToTenMiddleware>();
        }
    }
}
