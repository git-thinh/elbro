using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace System
{
    public static class Base64Utility
    {

        public static string f_Base64ToString(this string text)
        {
            return HttpUtility.UrlDecode(System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(text)));
        }

        public static string f_StringToBase64(this string text)
        {
            char[] chars = HttpUtility.HtmlEncode(text).ToCharArray();
            StringBuilder result = new StringBuilder(text.Length + (int)(text.Length * 0.1));

            foreach (char c in chars)
            {
                int value = Convert.ToInt32(c);
                if (value > 127)
                    result.AppendFormat("&#{0};", value);
                else
                    result.Append(c);
            }

            //return result.ToString();
            return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(result.ToString()));
        }

        public static byte[] GetByteArrayFromIntArray(this int[] intArray)
        {
            byte[] data = new byte[intArray.Length * 4];
            for (int i = 0; i < intArray.Length; i++)
                Array.Copy(BitConverter.GetBytes(intArray[i]), 0, data, i * 4, 4);
            return data;
        }

    }
}
