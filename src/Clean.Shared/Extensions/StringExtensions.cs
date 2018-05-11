// SOLUTION: Clean
// PROJECT: Clean.Shared
// FILE: StringExtensions.cs
// CREATED: Mike Gardner

namespace Clean.Shared.Extensions
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;

    #region ENUMERATIONS

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Values that represent random string contents. </summary>
    ///
    /// <remarks>   Mike Gardner - SurgeForward.com, 12/05/16. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public enum RandomStringContent
    {
        /// <summary>   An enum constant representing the alpha option. </summary>
        LettersOnly = 0,

        /// <summary>   An enum constant representing the numeric option. </summary>
        DigitsOnly = 1,

        /// <summary>   An enum constant representing the alphanumeric option. </summary>
        AlphaNumeric = 2
    }

    #endregion

    /// <summary>   A string extensions. </summary>
    public static class StringExtensions
    {
        #region Other Members

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Generates a random string of a desired length. </summary>
        ///
        /// <remarks>
        /// Mike Gardner - SurgeForward.com, 12/05/16. The random string is not guaranteed to be unique.
        /// </remarks>
        ///
        /// <throwses cref="ArgumentOutOfRangeException">
        /// Thrown when one or more arguments are outside the required range.
        /// </throwses>
        ///
        /// <param name="content">  The content. </param>
        /// <param name="length">   (Optional) The length. </param>
        ///
        /// <returns>   The random string, or null on error. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static string GenerateRandomString(RandomStringContent content, uint length = 10)
        {
            var permittedCharacters = string.Empty;

            switch (content)
            {
                case RandomStringContent.LettersOnly:
                    permittedCharacters = kLetterCharacters;
                    break;
                case RandomStringContent.DigitsOnly:
                    permittedCharacters = kDigitCharacters;
                    break;
                case RandomStringContent.AlphaNumeric:
                    permittedCharacters = kAlphaNumericCharacters;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(content), content, null);
            }

            return new string(Enumerable

                              // Permitted characters and length
                              .Repeat(permittedCharacters, (int) length)
                              .Select(s =>
                              {
                                  var cryptoResult = new byte[4];
                                  new RNGCryptoServiceProvider().GetBytes(cryptoResult);
                                  return s[new Random(BitConverter.ToInt32(cryptoResult, 0)).Next(s.Length)];
                              })
                              .ToArray());
        }

        #endregion

        #region CONSTANTS

        /// <summary>   The alpha numeric characters. </summary>
        private const string kAlphaNumericCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        /// <summary>   The digit characters. </summary>
        private const string kDigitCharacters = "0123456789";

        /// <summary>   The letter characters. </summary>
        private const string kLetterCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        #endregion
    }
}