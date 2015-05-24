

namespace ALOS.Expand
{
    public static class DoubleExpand
    {
        /// <summary>
        /// value-value2小于0返回0
        /// </summary>
        /// <param name="value"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static double Minus(this double value, double value2)
        {
            return value-value2>0?value-value2:0D;
        }
        /// <summary>
        /// value小于value2就返回value2否则返回value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static double IsLess(this double value, double value2)
        {
            return value < value2 ? value2 : value;
        }
    }
}
