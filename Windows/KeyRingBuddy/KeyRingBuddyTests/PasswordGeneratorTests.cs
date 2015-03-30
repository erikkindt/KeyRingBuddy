using KeyRingBuddy.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyRingBuddyTests
{
    /// <summary>
    /// Tests for the PasswordGenerator class.
    /// </summary>
    [TestClass]
    public class PasswordGeneratorTests
    {
        /// <summary>
        /// Test the Generate method.
        /// </summary>
        [TestMethod]
        public void PasswordGenerator_GenerateTest()
        {
            Console.WriteLine(PasswordGenerator.Generate(8, true, true, true, true));
            Console.WriteLine(PasswordGenerator.Generate(8, true, true, true, true));
            Console.WriteLine(PasswordGenerator.Generate(8, true, true, true, true));
            Console.WriteLine(PasswordGenerator.Generate(8, true, true, true, true));
            Console.WriteLine(PasswordGenerator.Generate(8, true, true, true, true));
        }
    }
}
