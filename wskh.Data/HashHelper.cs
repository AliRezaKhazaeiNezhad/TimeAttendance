using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wskh.Data
{
    public static class HashHelper
    {
        public static string Encrypt(String cipherText)
        {
            Encoding encoding = System.Text.Encoding.Unicode;
            string lastChar = cipherText[cipherText.Length - 1].ToString();

            Byte[] stringBytes = encoding.GetBytes(cipherText);
            StringBuilder sbBytes = new StringBuilder(stringBytes.Length * 2);
            foreach (byte b in stringBytes)
            {
                sbBytes.AppendFormat("{0:X2}", b);
            }
            return sbBytes.ToString() + lastChar;
        }
        public static string Decrypt(String cipherText)
        {
            Encoding encoding = System.Text.Encoding.Unicode;
            if (!string.IsNullOrEmpty(cipherText))
            {
                cipherText = cipherText.Substring(0, cipherText.Length - 1);

                int numberChars = cipherText.Length;
                byte[] bytes = new byte[numberChars / 2];
                for (int i = 0; i < numberChars; i += 2)
                {
                    bytes[i / 2] = Convert.ToByte(cipherText.Substring(i, 2), 16);
                }
                return encoding.GetString(bytes);
            }
            else
                return "";
        }
    }
}
