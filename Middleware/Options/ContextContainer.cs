using System.Threading;
using Infrastructure.Core;

namespace Middleware.Options
{
    public class ContextContainer
    {
        private static readonly AsyncLocal<IContextProperties> ContextCurrent = new AsyncLocal<IContextProperties>();

        public static IContextProperties Context
        {
            set => ContextCurrent.Value = value;
        }
    }
}