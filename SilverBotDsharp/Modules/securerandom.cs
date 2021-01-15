using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace SilverBotDsharp.Modules
{
    public class RandomGenerator
    {
        private readonly RNGCryptoServiceProvider csp;

        public RandomGenerator()
        {
            csp = new RNGCryptoServiceProvider();
        }

        public int Next(int minValue, int maxExclusiveValue)
        {
            if (minValue >= maxExclusiveValue)
            {
                ArgumentOutOfRangeException argumentOutOfRangeException = new ArgumentOutOfRangeException(nameof(minValue), "minValue must be lower than maxExclusiveValue");
                throw argumentOutOfRangeException;
            }

            long diff = (long)maxExclusiveValue - minValue;
            long upperBound = uint.MaxValue / diff * diff;

            uint ui;
            do
            {
                ui = GetRandomUInt();
            } while (ui >= upperBound);
            return (int)(minValue + (ui % diff));
        }

        private uint GetRandomUInt()
        {
            byte[] randomBytes = GenerateRandomBytes(sizeof(uint));
            return BitConverter.ToUInt32(randomBytes, 0);
        }

        private byte[] GenerateRandomBytes(int bytesNumber)
        {
            byte[] buffer = new byte[bytesNumber];
            csp.GetBytes(buffer);
            return buffer;
        }

        /// <summary>
        /// warning returns base64 might want to use RandomABCString
        /// </summary>
        /// <param name="length">how much bytes m8</param>
        /// <returns>a base64 thingy</returns>
        public string RandomString(int length)
        {
            byte[] tokenData = GenerateRandomBytes(length);
            return Convert.ToBase64String(tokenData);
        }

        public string RandomABCString(int length, double timespan = 1.5)
        {
            try
            {
                return Regex.Replace(RandomString(length), @"[^\w]", "",
                                       RegexOptions.None, TimeSpan.FromSeconds(timespan));
            }
            // If we timeout when replacing invalid characters,
            // we should return Empty.
            catch (RegexMatchTimeoutException)
            {
                return string.Empty;
            }
        }
    }
}