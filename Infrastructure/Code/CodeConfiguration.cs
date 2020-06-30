using System.Collections.Generic;

namespace Infrastructure.Code
{
    public class CodeConfiguration
    {
        public Dictionary<string, DataProps> Data { get; set; }
    }

    public class DataProps
    {
        public int LifeTime { get; set; }
        public int CodeLength { get; set; }
    }
}