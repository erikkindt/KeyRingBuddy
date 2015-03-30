using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KeyRingBuddy.Framework
{
    /// <summary>
    /// A password generator.
    /// </summary>
    public class PasswordGenerator
    {
        #region Fields

        /// <summary>
        /// Lower case letters.
        /// </summary>
        private static char[] LOWER_CASE_LETTERS = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        /// <summary>
        /// Upper case letters.
        /// </summary>
        private static char[] UPPER_CASE_LETTERS = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        /// <summary>
        /// Numbers.
        /// </summary>
        private static char[] NUMBERS = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        /// <summary>
        /// Punctuation marks.
        /// </summary>
        private static char[] PUNCTUATION = new char[] { '\'', '[', ']', '(', ')', '{', '}', ':', ',', '-', '_', '!', '.', '?', ';', '/', '\\', '&', '*', '@', '+' };

        #endregion

        #region Methods

        /// <summary>
        /// Generate a password.
        /// </summary>
        /// <param name="length">The length of the password to generate.</param>
        /// <param name="includeLowerCaseLetters">If true, lower case letters are included in the password.</param>
        /// <param name="includeUpperCaseLetters">If true, upper case letters are included in the password.</param>
        /// <param name="includeNumbers">If true, numbers are included in the password.</param>
        /// <param name="includePunctuation">If true, punctuations are included in the password.</param>
        /// <returns>The password that is generated.</returns>
        public static string Generate(
            int length,
            bool includeLowerCaseLetters,
            bool includeUpperCaseLetters,
            bool includeNumbers,
            bool includePunctuation)
        {
            if (length <= 0)
                throw new ArgumentException("length must be greater than zero.", "password");
            if (!includeLowerCaseLetters && !includeUpperCaseLetters && !includeNumbers && !includePunctuation)
                throw new ArgumentException("At least one of the following parameters must be true: includeLowerCaseLetters, includeUpperCaseLetters, includeNumbers, includePunctuation", "includeLowerCaseLetters/includeUpperCaseLetters/includeNumbers/includePunctuation");

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                int charType = Random(crypto, 4);
                bool charTypeValid = false;
                List<char> characters = new List<char>();

                for (int i = 0; i < length; i++)
                {
                    // identify the next type of character
                    charTypeValid = false;
                    while (!charTypeValid)
                    {
                        charType++;
                        if (charType >= 4)
                            charType = 0;

                        switch (charType)
                        {
                            case 0:
                                charTypeValid = includeLowerCaseLetters;
                                break;

                            case 1:
                                charTypeValid = includeUpperCaseLetters;
                                break;

                            case 2:
                                charTypeValid = includeNumbers;
                                break;

                            case 3:
                                charTypeValid = includePunctuation;
                                break;

                            default:
                                throw new Exception("(1)Invalid charType encountered: " + charType);
                        }
                    }

                    // get a new character of the appropriate type
                    switch (charType)
                    {
                        case 0:
                            characters.Add(LOWER_CASE_LETTERS[Random(crypto, LOWER_CASE_LETTERS.Length)]);
                            break;

                        case 1:
                            characters.Add(UPPER_CASE_LETTERS[Random(crypto, UPPER_CASE_LETTERS.Length)]);
                            break;

                        case 2:
                            characters.Add(NUMBERS[Random(crypto, NUMBERS.Length)]);
                            break;

                        case 3:
                            characters.Add(PUNCTUATION[Random(crypto, PUNCTUATION.Length)]);
                            break;

                        default:
                            throw new Exception("(2)Invalid charType encountered: " + charType);
                    }
                }

                // assemble the result
                StringBuilder result = new StringBuilder();
                while (characters.Count > 0)
                {
                    int index = Random(crypto, characters.Count);
                    result.Append(characters[index]);
                    characters.RemoveAt(index);
                }

                return result.ToString();
            }
        }

        /// <summary>
        /// Get a random integer between 0 and range (non-inclusive).
        /// </summary>
        /// <param name="crypto">The crypto to use for the random number generation.</param>
        /// <param name="range">The upper range but not including it.</param>
        /// <returns>A random integer between 0 and the given range.</returns>
        private static int Random(RNGCryptoServiceProvider crypto, int range)
        {
            byte[] randomNumber = new byte[1];
            crypto.GetBytes(randomNumber);
            return (randomNumber[0] % range);
        }

        #endregion
    }
}
