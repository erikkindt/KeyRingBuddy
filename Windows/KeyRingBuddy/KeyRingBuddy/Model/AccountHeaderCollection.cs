using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace KeyRingBuddy.Model
{
    /// <summary>
    /// A collection of account headers.
    /// </summary>
    public class AccountHeaderCollection : Collection<AccountHeader>, IXmlSerializable
    {
        #region IXmlSerializable Members

        /// <summary>
        /// Not used.
        /// </summary>
        /// <returns>null.</returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Read the list in from xml.
        /// </summary>
        /// <param name="reader">The reader to read from.</param>
        public void ReadXml(XmlReader reader)
        {
            reader.Read();

            while (reader.IsStartElement() && reader.LocalName == "item")
            {
                AccountHeader header = new AccountHeader();
                header.ReadXml(reader);
                this.Add(header);
            }
        }

        /// <summary>
        /// Write the list out to xml.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        public void WriteXml(XmlWriter writer)
        {
            foreach (AccountHeader header in this)
            {
                writer.WriteStartElement("item");
                header.WriteXml(writer);
                writer.WriteEndElement();
            }
        }

        #endregion
    }
}
