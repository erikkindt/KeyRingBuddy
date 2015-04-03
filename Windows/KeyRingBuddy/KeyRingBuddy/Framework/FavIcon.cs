using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;

namespace KeyRingBuddy.Framework
{
    /// <summary>
    /// A FavIcon downloaded from a website.
    /// </summary>
    public class FavIcon : IXmlSerializable
    {
        #region Properties

        /// <summary>
        /// A large version of the icon which is 32x32 pixels.
        /// </summary>
        public ImageSource LargeIcon { get; private set; }

        /// <summary>
        /// The default large icon.
        /// </summary>
        public static ImageSource LargeDefaultIcon
        {
            get { return VisualHelper.LoadBitmap("SiteLarge.png"); }
        }

        /// <summary>
        /// A large icon or the default if the large icon is null.
        /// </summary>
        public ImageSource LargeIconOrDefault
        {
            get
            {
                return LargeIcon ?? SmallIcon ?? LargeDefaultIcon;
            }
        }

        /// <summary>
        /// A small version of the icon which is 16x16 pixels.
        /// </summary>
        public ImageSource SmallIcon { get; private set; }

        /// <summary>
        /// The default small icon.
        /// </summary>
        public static ImageSource SmallDefaultIcon
        {
            get { return VisualHelper.LoadBitmap("SiteSmall.png"); }
        }

        /// <summary>
        /// A small version of the icon or the default if the small icon is null.
        /// </summary>
        public ImageSource SmallIconOrDefault
        {
            get
            {
                return SmallIcon ?? LargeIcon ?? SmallDefaultIcon;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Download FavIcon from a website.
        /// </summary>
        /// <param name="url">The url to download the FavIcon from.</param>
        /// <returns>The downloaded FavIcon or an empty icon if the download fails.</returns>
        public static FavIcon Download(string url)
        {
            if (String.IsNullOrWhiteSpace(url))
                throw new ArgumentException("url is null or whitespace.", "url");

            FavIcon result = new FavIcon();

            Uri uri = new Uri(url);
            string root = String.Format("{0}://{1}", uri.Scheme, uri.Host);

            byte[] icon = null;
            string iconType = null;

            using (WebClient webClient = new WebClient())
            {               
                // set the headers
                webClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.101 Safari/537.36");

                // get icon specified in file
                try
                {
                    string html = webClient.DownloadString(url);
                    MatchCollection matches = Regex.Matches(
                        html,
                        "<link\\b[^>]*", 
                        RegexOptions.IgnoreCase);

                    foreach (Match match in matches)
                    {
                        if (Regex.IsMatch(match.Value, "rel=\\\"(shortcut )?icon\\\"", RegexOptions.IgnoreCase))
                        {
                            Match hrefMatch = Regex.Match(
                                match.Value,
                                "href=\\\".*?\\\"",
                                RegexOptions.IgnoreCase);

                            if (hrefMatch != null)
                            {
                                int index = hrefMatch.Value.IndexOf('"');
                                string href = hrefMatch.Value.Substring(index + 1, hrefMatch.Value.Length - index - 2);

                                webClient.BaseAddress = root;
                                icon = webClient.DownloadData(href);
                                iconType = System.IO.Path.GetExtension(href).ToLower();

                                int indexQuestionMark = iconType.IndexOf('?');
                                if (indexQuestionMark != -1)
                                    iconType = iconType.Substring(0, indexQuestionMark);
                            }

                            if (Regex.IsMatch(match.Value, "rel=\\\"shortcut icon\\\"", RegexOptions.IgnoreCase))
                                break;
                        }
                    }
                }
                catch
                {
                    icon = null;
                    iconType = null;
                }

                // get icon at root level
                if (icon == null)
                {
                    try
                    {                        
                        string rootIcon = String.Format("{0}/favicon.ico", root);
                        icon = webClient.DownloadData(rootIcon);
                        iconType = ".ico";
                    }
                    catch
                    {
                        icon = null;
                        iconType = null;
                    }
                }
            }

            if (icon != null)
            {
                try
                {
                    // convert icon to bitmaps
                    using (System.IO.MemoryStream stream = new System.IO.MemoryStream(icon))
                    {
                        BitmapDecoder decoder = null;
                        switch (iconType)
                        {
                            case ".ico":
                                decoder = new IconBitmapDecoder(
                                    stream,
                                    BitmapCreateOptions.PreservePixelFormat,
                                    BitmapCacheOption.OnLoad);
                                break;

                            case ".gif":
                                decoder = new GifBitmapDecoder(
                                    stream,
                                    BitmapCreateOptions.PreservePixelFormat,
                                    BitmapCacheOption.OnLoad);
                                break;

                            case ".png":
                                decoder = new PngBitmapDecoder(
                                    stream,
                                    BitmapCreateOptions.PreservePixelFormat,
                                    BitmapCacheOption.OnLoad);
                                break;

                            default:
                                break;
                        }

                        if (decoder != null)
                        {
                            foreach (BitmapFrame frame in decoder.Frames)
                            {
                                if (frame.Height == 16 && frame.Width == 16)
                                    result.SmallIcon = frame;
                                else if (frame.Height == 32 && frame.Width == 32)
                                    result.LargeIcon = frame;
                            }
                        }
                    }
                }
                catch
                {
                }
            }

            return result;
        }

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
        /// Read in the fav icon.
        /// </summary>
        /// <param name="reader">The xml to read from.</param>
        public void ReadXml(XmlReader reader)
        {
            byte[] buffer = new byte[1024];
            int bitsRead = 0;

            reader.Read();

            while (reader.IsStartElement())
            {
                switch (reader.LocalName)
                {
                    case "smallIcon":
                        if (!reader.IsEmptyElement)
                        {
                            using (System.IO.MemoryStream smallIconStream = new System.IO.MemoryStream())
                            {
                                do
                                {
                                    bitsRead = reader.ReadElementContentAsBase64(buffer, 0, buffer.Length);
                                    if (bitsRead > 0)
                                        smallIconStream.Write(buffer, 0, bitsRead);
                                }
                                while (bitsRead > 0);

                                smallIconStream.Position = 0;
                                SmallIcon = BitmapFrame.Create(
                                    smallIconStream,
                                    BitmapCreateOptions.PreservePixelFormat,
                                    BitmapCacheOption.OnLoad);
                            }
                        }
                        else
                        {
                            reader.Read();
                        }
                        break;

                    case "largeIcon":
                        if (!reader.IsEmptyElement)
                        {
                            using (System.IO.MemoryStream largeIconStream = new System.IO.MemoryStream())
                            {
                                do
                                {
                                    bitsRead = reader.ReadElementContentAsBase64(buffer, 0, buffer.Length);
                                    if (bitsRead > 0)
                                        largeIconStream.Write(buffer, 0, bitsRead);
                                }
                                while (bitsRead > 0);

                                largeIconStream.Position = 0;
                                LargeIcon = BitmapFrame.Create(
                                    largeIconStream,
                                    BitmapCreateOptions.PreservePixelFormat,
                                    BitmapCacheOption.OnLoad);
                            }
                        }
                        else
                        {
                            reader.Read();
                        }
                        break;

                    default:
                        reader.Read();
                        break;
                }
            }

            reader.Read();
        }

        /// <summary>
        /// Write out the fav icon.
        /// </summary>
        /// <param name="writer">The xml to write to.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("smallIcon");
            BitmapFrame smallFrame = SmallIcon as BitmapFrame;
            if (smallFrame != null)
            {
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(smallFrame);
                    encoder.Save(ms);

                    ms.Flush();
                    byte[] bits = ms.ToArray();
                    writer.WriteBase64(bits, 0, bits.Length);
                }
            }
            writer.WriteEndElement();

            writer.WriteStartElement("largeIcon");
            BitmapFrame largeFrame = LargeIcon as BitmapFrame;
            if (largeFrame != null)
            {
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(largeFrame);
                    encoder.Save(ms);

                    ms.Flush();
                    byte[] bits = ms.ToArray();
                    writer.WriteBase64(bits, 0, bits.Length);
                }
            }
            writer.WriteEndElement();
        }

        #endregion
    }
}
