namespace RequestProcessingPipeline
{
    public static class FromTenThousandToFifteenThousandExtensions
    {
        public static IApplicationBuilder UseFromTenThousandToFifteenThousand(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FromTenThousandToFiftyThousandMiddleware>();
        }
    }
}
