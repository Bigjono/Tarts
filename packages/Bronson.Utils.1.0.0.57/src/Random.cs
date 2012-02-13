using System;
using RandGen = System.Random;
namespace Bronson.Utils
{
    public class Random
    {
        public static string RandomString(int length)
        {
            return RandomString(length, length);
        }

        /// <summary>
        /// Generates a random alpha numeric string
        /// </summary>
        public static string RandomString(int minLength, int maxLength)
        {
            RandGen tempRandGen = new RandGen(System.DateTime.Now.Millisecond);
            int length = tempRandGen.Next(minLength, maxLength);

            int MinNumber = 65; //65-90 equals A-Z
            int MaxNumber = 90; //49-57 equals 0-9

            RandGen r = new RandGen(System.DateTime.Now.Millisecond);
            int Let1 = r.Next(MinNumber, MaxNumber);

            string code = String.Empty;
            int i = 0;

            for (i = 0; i < length; i++)
            {
                int letter = r.Next(MinNumber, MaxNumber);
                code += ((char)letter).ToString();
            }

            return code;
        }
    }
}
