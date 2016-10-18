

using System;
using System.Security.Cryptography;
using System.Text;

namespace QMTech.Core
{
    public static class Extensions
    {
        public static bool IsNullOrDefault<T>(this T? value) where T : struct
        {
            return default(T).Equals(value.GetValueOrDefault());
        }
    }

    public static class StringExtensions
    {
        public static string GetHash(this string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();

            byte[] byteValue = Encoding.UTF8.GetBytes(input);

            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }

        //首字母转小写
        public static string toLowerCaseFirstOne(this string s)
        {
            return (new StringBuilder()).Append(s.Substring(0, 1).ToLower()).Append(s.Substring(1)).ToString();
        }
        //首字母转大写
        public static string toUpperCaseFirstOne(this string s)
        {
            return (new StringBuilder()).Append(s.Substring(0, 1).ToUpper()).Append(s.Substring(1)).ToString();
        }
    }
}
