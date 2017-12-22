using System;
using System.Text;
using System.Security.Cryptography;

namespace qcloudsms_csharp
{
    public static class SmsSenderUtil
    {
        public static long getCurrentTime()
        {
            return (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public static long getRandom()
        {
            return (new Random((int)SmsSenderUtil.getCurrentTime())).Next() % 900000L + 100000L;
        }

        public static string calculateSignature(string appkey, long random, long time,
            string phoneNumber)
        {
            StringBuilder builder = new StringBuilder("appkey=")
                .Append(appkey)
                .Append("&random=")
                .Append(random)
                .Append("&time=")
                .Append(time)
                .Append("&mobile=")
                .Append(phoneNumber);

            return sha256(builder.ToString());
        }

        public static string calculateSignature(string appkey, long random, long time,
            string[] phoneNumbers)
        {
            StringBuilder builder = new StringBuilder("appkey=")
                .Append(appkey)
                .Append("&random=")
                .Append(random)
                .Append("&time=")
                .Append(time)
                .Append("&mobile=");

            if (phoneNumbers.Length > 0)
            {
                builder.Append(phoneNumbers[0]);
                for (int i = 1; i < phoneNumbers.Length; i++)
                {
                    builder.Append(",");
                    builder.Append(phoneNumbers[i]);
                }
            }

            return sha256(builder.ToString());
        }

        public static string calculateSignature(string appkey, long random, long time)
        {
            StringBuilder builder = new StringBuilder("appkey=")
                .Append(appkey)
                .Append("&random=")
                .Append(random)
                .Append("&time=")
                .Append(time);

            return sha256(builder.ToString());
        }

        private static string sha256(string rawString)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(rawString);
            byte[] hash = SHA256Managed.Create().ComputeHash(bytes);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("X2"));
            }

            return builder.ToString();
        }
    }
}