using KeyRingBuddy.Framework;
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
    /// Contains less sensitive account information suitable for display in UI.
    /// </summary>
    public class AccountHeader : IXmlSerializable
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public AccountHeader()
        {
            Category = null;
            AccountName = null;
            AccountIcon = new FavIcon();
            AccountId = Guid.Empty;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="category">Category.</param>
        /// <param name="accountName">AccountName.</param>
        /// <param name="accountIcon">AccountIcon.</param>
        /// <param name="accountId">AccountId.</param>
        public AccountHeader(string category, string accountName, FavIcon accountIcon, Guid accountId)
        {
            Category = category;
            AccountName = accountName;
            AccountIcon = accountIcon ?? new FavIcon();
            AccountId = accountId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The category the account belongs to.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// The name of the account.
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// The account icon.
        /// </summary>
        public FavIcon AccountIcon { get; set; }

        /// <summary>
        /// The account id.
        /// </summary>
        public Guid AccountId { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Create a list of unique categories.
        /// </summary>
        /// <param name="profile">The profile to generate the list from.</param>
        /// <param name="includeAllCategory">If true, a category with the name All will be included at the beginning.</param>
        /// <returns>A sorted list of unique categories.</returns>
        public static IEnumerable<string> GetSortedUniqueCategoryNames(IProfile profile, bool includeAllCategory)
        {
            if (profile == null)
                throw new ArgumentNullException("profile");

            bool allCategoryExists = false;

            HashSet<string> categoryNames = new HashSet<string>();
            foreach (AccountHeader header in profile.GetAccountHeaders())
            {
                if (String.Compare(header.Category, "All", true) == 0)
                {
                    allCategoryExists = true;
                    continue;
                }

                categoryNames.Add(header.Category);
            }

            List<string> sortedCategoryNames = new List<string>(categoryNames);
            sortedCategoryNames.Sort();

            if (allCategoryExists || includeAllCategory)
                sortedCategoryNames.Insert(0, "All");

            return sortedCategoryNames;
        }

        /// <summary>
        /// Equality is based on AccountId.
        /// </summary>
        /// <param name="obj">The object to compare with this one.</param>
        /// <returns>true if the obj is logically equal to this object.</returns>
        public override bool Equals(object obj)
        {
            AccountHeader other = obj as AccountHeader;
            if (other == null)
                return false;

            return (other.AccountId == this.AccountId);
        }

        /// <summary>
        /// The hash code is based on the AccountId.
        /// </summary>
        /// <returns>The hash code for this object.</returns>
        public override int GetHashCode()
        {
            return AccountId.GetHashCode();
        }

        /// <summary>
        /// The AccountName property is returned.
        /// </summary>
        /// <returns>The AccountName.</returns>
        public override string ToString()
        {
            return AccountName;
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

            Category = reader["category"];
            AccountName = reader["accountName"];
            AccountId = Guid.Parse(reader["accountId"]);

            if (!reader.IsEmptyElement)
            {
                if (reader.ReadToDescendant("accountIcon"))
                {
                    AccountIcon = new FavIcon();
                    AccountIcon.ReadXml(reader);
                }
            }

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

            writer.WriteAttributeString("category", Category);
            writer.WriteAttributeString("accountName", AccountName);
            writer.WriteAttributeString("accountId", AccountId.ToString());

            writer.WriteStartElement("accountIcon");
            if (AccountIcon != null)
                AccountIcon.WriteXml(writer);
            writer.WriteEndElement();
        }

        #endregion
    }
}
