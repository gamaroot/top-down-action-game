using System;

namespace Utils
{
    public class EnumUtils<T> where T : struct, IConvertible
    {
        public static int Length()
        {
            return Enum.GetValues(typeof(T)).Length;
        }
    }
}