using System;

namespace Middleware.Options
{
    public class CorrelationIdOptions
    {
        public static string DefaultHeader = "X-Correlation-ID";
        private static readonly Func<string> DefaultGenerateValueFunc = (Func<string>) (() => Guid.NewGuid().ToString().Replace("-", string.Empty));

        public Func<string> GenerateValueFunc { get; set; } = CorrelationIdOptions.DefaultGenerateValueFunc;

        public bool IncludeInResponse { get; set; } = true;
    }
}