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
