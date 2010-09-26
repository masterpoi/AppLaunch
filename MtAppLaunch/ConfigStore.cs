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
                        Target = (string)item.Element("target"),
                        Icon = (string)item.Element("icon")
                    }).ToArray();
        }

        public static void Save(Item[] items)
        {
            var doc = new XDocument(new XElement("items", items.Select(item => item.ToXElement())));
            doc.Save(_ConfigUri);
        }

    }
}
