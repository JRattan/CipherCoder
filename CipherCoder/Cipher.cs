using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CipherCoder
{
    class Cipher
    {
        /*
         * The encryption process works like this:
         * Using Caesar's cipher, each ASCII code is shifted by 3.
         * The result yields a set of integers since some letters (such as 'X', 'Y', and 'Z' when shifted will no longer be a letter)
         * Next each digit of a letter will be converted to its own letter. So an integer 123 for example would become '1', '2', '3' which whould
         * then be converted as 'A', 'C', 'E' or 'B', 'D', 'F' or any combination of those.
         * Each integer would be separated by a 'U', 'V', 'W', 'X', 'Y', or 'Z'
         * Now that the set is now composed of letters, the program will be making use of Vignere cipher using the key entered.
         * Finally the resulting string will be reversed
         */
        public static string Encrypt(string input, string key)
        {
            if (!IsAllLetters(key))
                throw new Exception("Key must consist of only letters!");
            return ReverseString(Vignere(EnSecondPass(EnFirstPass(input)), key));
        }

        /*
         * The decryption process works like this:
         * First the string will be reversed back to normal
         * Vignere cipher will be implemented then 
         * The letters will then be converted back to numbers, if it's from 'U' to 'Z' then it is converted to 'X'
         * The numbers will then be combined to its original integer format. The program loops until it reaches an 'X'
         * Finally Caesar's cipher will be used and will be shifted by 3.
         * 
         */
        public static string Decrypt(string input, string key)
        {
            if (!IsAllLetters(input))
                throw new Exception("When decrypting, Input must only be composed of letters!");
            if (!IsAllLetters(key))
                throw new Exception("Key must consist of only letters!");

            return DeSecondPass(DeFirstPass(Vignere(ReverseString(input).ToUpper(), key, -1)));
        }

        private static bool IsAllLetters(string message)
        {
            foreach (char i in message)
            {
                if (i == '\n') continue;

                int j = (int)i;
                if (i < 'A' || (i > 'Z' && i < 'a') || i > 'z')
                {
                    return false;
                }
            }

            return true;
        }

        private static string ReverseString(string text)
        {
            char[] array = text.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }

        private static string EnFirstPass(string message)
        {
            string newMessage = "";

            foreach (char i in message)
            {
                newMessage += String.Format("{0}X", i + 3);
            }

            return newMessage;
        }

        private static string EnSecondPass(string message)
        {
            string newMessage = "";
            Random rand = new Random();

            foreach (char i in message)
            {
                int k = 0;
                if (i != 'X')
                {
                    k = (int)Char.GetNumericValue(i);

                    for (int j = 0; j < 10; j++)
                    {
                        if (k == j) newMessage += (char)(rand.Next('A' + (2 * j), 'C' + (2 * j)));
                    }
                }
                else newMessage += (char)(rand.Next(85, 91));
            }

            return newMessage;
        }

        private static string DeFirstPass(string message)
        {
            string newMessage = "";

            foreach (char i in message)
            {
                if (i == '\n') continue;

                for (int j = 0; j < 10; j++)
                {
                    if ((int)i == ('A' + (2 * j)) || (int)i == ('B' + (2 * j))) newMessage += String.Format("{0}", j);
                }
                if (i >= 'U' && i <= 'Z') newMessage += 'X';
            }

            return newMessage;
        }

        private static string DeSecondPass(string message)
        {
            string newMessage = "";
            string digit = "";

            foreach (char i in message)
            {
                if (i == '\n') continue;

                if (i == 'X')
                {
                    newMessage += String.Format("{0}", (char)(Int32.Parse(digit) - 3));
                    digit = "";
                }
                else digit += i;
            }

            return newMessage;
        }

        private static string Vignere(string message, string keyword, int encrypt = 1)
        {
            string newMessage = "";

            message = message.ToUpper();

            for (int i = 0; i < message.Length; i++)
            {
                if (message[i] == '\n') continue;

                int vt = (int)message[i];
                int vk = (int)keyword[i % keyword.Length];

                int vr = vt + (vk * encrypt);
                if (vr < 0) vr += 26;
                newMessage += (char)((vr % 26) + 'A');
            }

            return newMessage;
        }
    }
}
