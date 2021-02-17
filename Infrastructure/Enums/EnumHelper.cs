using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Enums
{
    public class EnumHelper
    {
        public static IEnumerable<Enum> GetFlags(Enum input)
        {
            foreach (Enum value in Enum.GetValues(input.GetType()))
                if (input.HasFlag(value))
                    yield return value;
        }
    }
}
