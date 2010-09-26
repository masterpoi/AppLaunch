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
    public abstract class Item : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public abstract XElement ToXElement();

        public abstract ImageSource Image { get; }
    }



    public class ShortCut : Item
    {
        public string Target { get; set; }

        public override XElement ToXElement()
        {
            return new XElement("item", new XElement("target", Target), new XAttribute("type", "shortcut"));

        }

        public override ImageSource Image
        {
            get
            {
                var ico = Icon.ExtractAssociatedIcon(Target);
                MemoryStream strm = new MemoryStream();
                ico.Save(strm);
                IconBitmapDecoder ibd = new IconBitmapDecoder(strm, BitmapCreateOptions.None, BitmapCacheOption.Default);
                return ibd.Frames[0];

            }
        }
    }

    public static class ConfigStore
    {

        private static readonly string _ConfigUri = ConfigurationManager.AppSettings["ConfigUri"];

        public static Item[] Read()
        {
            var xdoc = XDocument.Load(_ConfigUri);

            return (from item in xdoc.Descendants("item")
                    where item.Attribute("type").Value == "shortcut"
                    select new ShortCut
                    {
                        Target = (string)item.Element("target")
                    }).ToArray();
        }

        public static void Save(Item[] items)
        {
            var doc = new XDocument(new XElement("items", items.Select(item => item.ToXElement())));
            doc.Save(_ConfigUri);
        }

    }
}
