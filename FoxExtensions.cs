using System;
using System.Collections.Generic;
using System.Text;

namespace ThreadFileReader
{
    public static class FoxExtensions
    {
        public static byte[] ToBytes(this string arg)
        {
            return Encoding.UTF8.GetBytes(arg);
        }
    }
}

