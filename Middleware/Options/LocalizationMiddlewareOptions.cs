namespace Middleware.Options
{
    public class LocalizationMiddlewareOptions
    {
        public const string DefaultLanguageHeaderName = "language";

        public string LanguageHeaderName { get; set; } = "language";
    }
}