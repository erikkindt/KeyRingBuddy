using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace KeyRingBuddy.Framework
{
    /// <summary>
    /// Adds extension methods to SecureString.
    /// </summary>
    public static class SecureStringUtility
    {
        /// <summary>
        /// Gets the plain text from a SecureString instance.
        /// </summary>
        /// <param name="secureString">The secure string to get the plain text from.</param>
        /// <returns>The plain text from the secure string.</returns>
        public static string GetPlainText(SecureString secureString)
        {
            if (secureString == null)
                throw new ArgumentNullException("secureString");

            IntPtr bstr = Marshal.SecureStringToBSTR(secureString);
            string text = Marshal.PtrToStringBSTR(bstr);
            Marshal.FreeBSTR(bstr);

            return text;
        }

        /// <summary>
        /// Create a new secure string.
        /// </summary>
        /// <param name="text">The text to create a secure string with.</param>
        /// <returns>A new secure string.</returns>
        public static SecureString CreateSecureString(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            SecureString secureString = new SecureString();
            foreach (char c in text)
                secureString.AppendChar(c);

            return secureString;
        }
    }
}
