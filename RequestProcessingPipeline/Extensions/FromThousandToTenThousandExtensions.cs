﻿using RequestProcessingPipeline.Middlewares;

namespace RequestProcessingPipeline.Extensions
{
    public static class FromThousandToTenThousandExtensions
    {
        public static IApplicationBuilder UseFromThousandToTenThousand(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FromThousandToTenThousandMiddleware>();
        }
    }
}
