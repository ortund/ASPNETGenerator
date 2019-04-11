using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDGenerator.Extensions
{
    public static class StringExtensions
    {
        public static string LowerFirstChar(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return $"{input.First().ToString().ToLower()}{input.Substring(1)}";
        }
    }
}
