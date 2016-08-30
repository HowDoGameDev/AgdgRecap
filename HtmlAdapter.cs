﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AgdgRecap
{
    class HtmlAdapter
    {
        public static List<AgdgEntry> GetEntries(string html, bool fullPath = true)
        {
            if (!fullPath)
                html = @"C:\Users\phillip\Documents\GameResearch\Recap\Recaps\" + html + @"\" + html + ".html";

            List<AgdgEntry> entries = new List<AgdgEntry>();

            XDocument xDoc = XDocument.Load(html);
            
            foreach (XElement xElem in xDoc.Descendants("div").Where(x => x.Attribute("class") != null && x.Attribute("class").Value == "agdgEntry"))
            {
                entries.Add(AgdgEntry.CreateEntry(xElem, System.IO.Path.GetDirectoryName(html)));
            }

            return entries;
        }        
    }
}
