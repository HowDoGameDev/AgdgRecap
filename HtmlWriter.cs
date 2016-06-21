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
        public static void WriteHtml(List<AgdgEntry> entries, string htmlName)
        {
            XDocument xDoc = new XDocument();
            XElement xHtml = new XElement("html");
            GenerateCss(xHtml);
            XElement xBody = new XElement("body");
            xHtml.Add(xBody);
            xDoc.Add(xHtml);

            AddHeader(xBody);

            int x = 0;
            int y = 0;
            XElement xRowDiv = new XElement("div");
            xRowDiv.Add(new XAttribute("class", "agdgRow"));

            var games = new List<string>();
            foreach (var e in entries)
            {
                if (e.Game.Contains("gnome"))
                    Console.WriteLine();

                if (games.Contains(e.Game))
                    continue;
                else
                    games.Add(e.Game);

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

            xDoc.Save(htmlName + ".html");
        }

        static void AddHeader(XElement xBody)
        {
            XElement xheadDiv = new XElement("div");
            xheadDiv.Add(new XAttribute("class", "agdgHeader"));

            XElement xTitDiv = new XElement("div");
            xTitDiv.Add(new XElement("h1", "AGDG Weekly Recap"));
            xTitDiv.Add(new XElement("h2", "June 2016 Week 4"));
            xheadDiv.Add(xTitDiv);

            var filePath = "logo.png";
            XElement xI = new XElement("img");
            xI.Add(new XAttribute("src", filePath));
            xheadDiv.Add(xI);

            xBody.Add(xheadDiv);
        }

        static void GenerateCss(XElement xDoc)
        {
            XElement xHead = new XElement("head");
            xDoc.Add(xHead);

            string link = "<link rel=\"stylesheet\" type=\"text/css\" href=\"style.css\"/>";
            XElement xLink = XElement.Parse(link);
            xHead.Add(xLink);

            //XElement xStyle = new XElement("style");
            //xHead.Add(xStyle);
            //xStyle.SetValue(File.ReadAllText("style.css"));
        }
    }
}
