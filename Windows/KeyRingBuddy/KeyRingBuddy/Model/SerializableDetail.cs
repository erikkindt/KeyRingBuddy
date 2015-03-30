using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace KeyRingBuddy.Model
{
    /// <summary>
    /// A value that can be serialized to xml.
    /// </summary>
    public class SerializableDetail : IXmlSerializable
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SerializableDetail()
        {
            Value = null;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">Value.</param>
        public SerializableDetail(string value)
        {
            Value = value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The value to serialize.
        /// </summary>
        public string Value { get; set; }

        #endregion

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
        /// Read the value in.
        /// </summary>
        /// <param name="reader">The reader to read from.</param>
        public void ReadXml(XmlReader reader)
        {
            Value = reader["value"];
        }

        /// <summary>
        /// Write the detail out.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("value", Value);
        }

        #endregion
    }
}
