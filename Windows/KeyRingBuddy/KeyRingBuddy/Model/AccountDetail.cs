using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace KeyRingBuddy.Model
{
    /// <summary>
    /// An account detail.
    /// </summary>
    public class AccountDetail : IXmlSerializable
    {
        #region Constructors

        /// <summary>
        /// Constructor used by IXmlSerializable process.
        /// </summary>
        public AccountDetail()
        {
            Name = null;
            Value = null;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        public AccountDetail(string name, string value)
        {
            Name = name;
            Value = value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The name of the account details.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The value of the account detail.
        /// </summary>
        public string Value { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the name and value of this detail.
        /// </summary>
        /// <returns>The name and value of this detail.</returns>
        public override string ToString()
        {
            return String.Format("{0} : {1}", Name, Value);
        }

        /// <summary>
        /// Checks object for equality with this object.
        /// </summary>
        /// <param name="obj">The object to check against this one.</param>
        /// <returns>true if the object is logically equal to this object, false if it isn't.</returns>
        public override bool Equals(object obj)
        {
            AccountDetail other = obj as AccountDetail;
            if (other == null)
                return false;

            return (this.Name == other.Name && this.Value == other.Value);
        }

        /// <summary>
        /// Checks two account details for equality.
        /// </summary>
        /// <param name="a">The first account detail to check.</param>
        /// <param name="b">The second account detail to check.</param>
        /// <returns>true if the two account detail are logically equal, false if they aren't.</returns>
        public static bool Equals(AccountDetail a, AccountDetail b)
        {
            if (a == null && b == null)
                return true;
            if (a == null || b == null)
                return false;

            return a.Equals(b);
        }

        /// <summary>
        /// The Name property is used for the hash code.
        /// </summary>
        /// <returns>The hash code of the Name property.</returns>
        public override int GetHashCode()
        {
            return Name == null ? 0 : Name.GetHashCode();
        }

        #endregion

        #region IXmlSerializable Members

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <returns>null.</returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Read in account detail.
        /// </summary>
        /// <param name="reader">The reader that the account detail is read from.</param>
        public void ReadXml(XmlReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            Name = reader.GetAttribute("name");
            Value = reader.GetAttribute("value");
        }

        /// <summary>
        /// Write this account detail to the xml writer.
        /// </summary>
        /// <param name="writer">The writer that this detail is written to.</param>
        public void WriteXml(XmlWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.WriteAttributeString("name", Name);
            writer.WriteAttributeString("value", Value);
        }

        #endregion
    }
}
