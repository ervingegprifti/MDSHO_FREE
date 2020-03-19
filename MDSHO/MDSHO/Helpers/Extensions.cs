using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace MDSHO.Helpers
{
    public static class Extensions
    {



        #region COLOR CONVERTERS

        /// <summary>
        /// Convert a Color to int.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static int ToInt(this System.Windows.Media.Color colorToInt)
        {
            byte[] bytes = new byte[4];
            bytes[0] = colorToInt.B;
            bytes[1] = colorToInt.G;
            bytes[2] = colorToInt.R;
            bytes[3] = colorToInt.A;
            int intFromColor = BitConverter.ToInt32(bytes, 0);
            return intFromColor;
        }
        /// <summary>
        /// Convert a Color to SolidColorBrush.
        /// </summary>
        /// <param name="colorToSolidColorBrush"></param>
        /// <returns></returns>
        public static SolidColorBrush ToSolidColorBrush(this System.Windows.Media.Color colorToSolidColorBrush)
        {
            return new SolidColorBrush { Color = colorToSolidColorBrush };
        }
        /// <summary>
        /// Convert an int to Color.
        /// </summary>
        /// <param name="intFromColor"></param>
        /// <returns></returns>
        public static System.Windows.Media.Color ToColor(this int intToColor)
        {
            byte[] bytes = BitConverter.GetBytes(intToColor);
            System.Windows.Media.Color colorFromInt = new System.Windows.Media.Color()
            {
                B = bytes[0],
                G = bytes[1],
                R = bytes[2],
                A = bytes[3]
            };
            return colorFromInt;
        }

        #endregion

    }
}
