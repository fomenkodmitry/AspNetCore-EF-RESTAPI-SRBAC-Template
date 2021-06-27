using System.Collections.Generic;

namespace Middleware.Options
{
    public class LogRequestResponseOptions
    {
        public bool LogRequest { get; set; }

        public bool LogResponse { get; set; }

        public ICollection<string> ExcludeRequestPaths { get; set; } = (ICollection<string>) new List<string>()
        {
            "swagger",
            "version",
        };
    }
}