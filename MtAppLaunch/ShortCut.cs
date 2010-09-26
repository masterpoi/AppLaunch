using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Linq;
using System.Configuration;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;

namespace MtAppLaunch
{
    public class ShortCut : Item
    {
        public string Target { get; set; }
        public string Icon { get; set; }

        public override XElement ToXElement()
        {
            return new XElement("item", new XElement("target", Target), new XElement("icon", Icon), new XAttribute("type", "shortcut"));

        }

        private ImageSource GetImageFromAssociatedIcon()
        {
            var ico = System.Drawing.Icon.ExtractAssociatedIcon(Target);
            MemoryStream strm = new MemoryStream();
            ico.Save(strm);
            IconBitmapDecoder ibd = new IconBitmapDecoder(strm, BitmapCreateOptions.None, BitmapCacheOption.Default);
            return ibd.Frames[0];
        }
        public override ImageSource Image
        {
            get
            {
                if (Icon != null)
                {
                    var uriSource = new Uri(Icon, UriKind.Relative);
                    return new BitmapImage(uriSource);

                }
                return GetImageFromAssociatedIcon();

            }
        }
    }
}
