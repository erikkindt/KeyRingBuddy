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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace KeyRingBuddy.Model
{
    /// <summary>
    /// An account.
    /// </summary>
    public class Account : IXmlSerializable
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public Account()
        {
            Id = Guid.NewGuid();
            Name = null;
            Category = null;
            Site = null;
            Icon = new FavIcon();
            Details = new List<AccountDetail>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="name">Name.</param>
        /// <param name="category">Category.</param>
        /// <param name="site">Site.</param>
        public Account(string name, string category, string site)
        {
            Id = Guid.NewGuid();
            Name = name;
            Category = category;
            Site = site;
            Icon = new FavIcon();
            Details = new List<AccountDetail>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The id of this account.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// The name of this account.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The category this account belongs to.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// The site for this account which may be a website or an application file.
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// The account icon.
        /// </summary>
        public FavIcon Icon { get; set; }

        /// <summary>
        /// The details for this account.
        /// </summary>
        public IList<AccountDetail> Details { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the Id property.
        /// </summary>
        /// <returns>The Id property.</returns>
        public override string ToString()
        {
            return Id.ToString();
        }

        /// <summary>
        /// Check to see if the given object is equal to this object.
        /// </summary>
        /// <param name="obj">The object to check against this one.</param>
        /// <returns>true if the object is logically equal to this one.</returns>
        public override bool Equals(object obj)
        {
            Account other = obj as Account;
            if (other == null)
                return false;

            return (this.Id == other.Id);
        }

        /// <summary>
        /// Checks the two category objects for equality.
        /// </summary>
        /// <param name="a">The first object to check.</param>
        /// <param name="b">The second object to check.</param>
        /// <returns>true if the two objects are logically equal, false if they aren't.</returns>
        public static bool Equals(Account a, Account b)
        {
            if (a == null && b == null)
                return true;
            if (a == null || b == null)
                return false;

            return (a.Equals(b));
        }

        /// <summary>
        /// The Name property is used for the hash code.
        /// </summary>
        /// <returns>The hash code of the Name property.</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
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
        /// Read in account.
        /// </summary>
        /// <param name="reader">The reader that the account is read from.</param>
        public void ReadXml(XmlReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            Id = Guid.Parse(reader["id"]);
            Name = reader["name"];
            Category = reader["category"];
            Site = reader["site"];

            Details.Clear();
            if (reader.ReadToDescendant("details") &&
                reader.ReadToDescendant("detail"))
            {
                do
                {
                    AccountDetail ad = new AccountDetail();
                    ad.ReadXml(reader);
                    Details.Add(ad);
                } 
                while (reader.ReadToNextSibling("detail"));

                reader.Read();
            }

            Icon = new FavIcon();
            if (reader.LocalName == "icon" && !reader.IsEmptyElement)
                Icon.ReadXml(reader);

            reader.Read();
        }

        /// <summary>
        /// Write this account to the xml writer.
        /// </summary>
        /// <param name="writer">The writer that this detail is written to.</param>
        public void WriteXml(XmlWriter writer)
        {
            if (writer == null)
                throw new ArgumentException("writer");

            writer.WriteAttributeString("id", Id.ToString());
            writer.WriteAttributeString("name", Name);
            writer.WriteAttributeString("category", Category);
            writer.WriteAttributeString("site", Site);

            writer.WriteStartElement("details");
            foreach (AccountDetail detail in Details)
            {
                writer.WriteStartElement("detail");
                detail.WriteXml(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement("icon");
            if (Icon != null)
                Icon.WriteXml(writer);
            writer.WriteEndElement();
        }

        #endregion
    }
}
