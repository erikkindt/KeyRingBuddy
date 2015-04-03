/*
 * Copyright (c) 2015 Nathaniel Wallace
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using KeyRingBuddy.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KeyRingBuddyTests
{
    /// <summary>
    /// Tests for the FavIcon class.
    /// </summary>
    [TestClass]
    public class FavIconTests
    {
        /// <summary>
        /// Test the download method.
        /// </summary>
        [TestMethod]
        public void FavIcon_DownloadTest()
        {
            FavIcon fi = FavIcon.Download("https://www.google.com");
            Assert.IsNotNull(fi.SmallIcon);
            Assert.IsNotNull(fi.LargeIcon);

            FavIcon fiFail = FavIcon.Download("http://www.fail12345ok.com");
            Assert.IsNull(fiFail.SmallIcon);
            Assert.IsNull(fiFail.LargeIcon);
        }

        /// <summary>
        /// Test the serialization methods.
        /// </summary>
        [TestMethod]
        public void FavIcon_SerializeTest()
        {
            FavIcon fi = FavIcon.Download("https://www.google.com");
            Assert.IsNotNull(fi.SmallIcon);
            Assert.IsNotNull(fi.LargeIcon);

            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer ser = new XmlSerializer(typeof(FavIcon));
                ser.Serialize(ms, fi);
                ms.Position = 0;

                string xml = Encoding.ASCII.GetString(ms.ToArray());

                FavIcon other = ser.Deserialize(ms) as FavIcon;
                Assert.IsNotNull(other.SmallIcon);
                Assert.IsNotNull(other.LargeIcon);
            }
        }
    }
}
