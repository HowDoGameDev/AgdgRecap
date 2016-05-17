using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AgdgRecap
{
    class HtmlWriter
    {
        public static void WriteHtml(List<AgdgEntry> entries)
        {
            XDocument xDoc = new XDocument();
            XElement xHtml = new XElement("html");
            GenerateCss(xHtml);
            XElement xBody = new XElement("body");
            xHtml.Add(xBody);
            xDoc.Add(xHtml);

            int x = 0;
            int y = 0;
            XElement xRowDiv = new XElement("div");
            xRowDiv.Add(new XAttribute("class", "agdgRow"));
            foreach (var e in entries)
            {
                xRowDiv.Add(e.GetHtml());                

                if (++x >= 4)
                {
                    x = 0; y++;
                    xBody.Add(xRowDiv);
                    xRowDiv = new XElement("div");
                    xRowDiv.Add(new XAttribute("class", "agdgRow"));
                }
            }
            xBody.Add(xRowDiv); 

            xDoc.Save("test.html");
        }

        static void GenerateCss(XElement xDoc)
        {
            XElement xHead = new XElement("head");
            xDoc.Add(xHead);
            
            XElement xStyle = new XElement("style");
            xHead.Add(xStyle);
            xStyle.SetValue(File.ReadAllText("style.css"));
        }
    }
}
