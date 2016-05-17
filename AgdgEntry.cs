﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AgdgRecap
{
    class AgdgEntry
    {
        public string Game;
        public string Dev;
        public string Tools;
        public string Web;
        public List<string> GoodProgress;
        public List<string> BadProgress;
        bool _isValid = false;
        string _id;
        string _postId;
        string _imageUrl;
        string _imageName;

        AgdgEntry(JToken jObject, string postId)
        {
            _postId = postId;

            _id = jObject["no"].ToString();
            var comment = jObject["com"].ToString();
            var gIdx = comment.IndexOf("Game:");
            if (gIdx == -1)
                return;

            var lines = comment.Substring(gIdx).Replace("<br>", "♫").Split('♫');
            Game = GetValue(lines, "Game:");
            Dev = GetValue(lines, "Dev:");
            Tools = GetValue(lines, "Tools:");
            Web = GetValue(lines, "Web:");
            GetProgress(lines);

            if (Game == null || Dev == null || Tools == null || Web == null)
                return;
            
            //Build image path
            var jtim = jObject["tim"];
            if (jtim == null)
                return;
            _imageName = jtim.ToString() + jObject["ext"].ToString();
            if (Path.GetExtension(_imageName) == ".webm") //Just use the thumbnail if WebM.
                _imageName = jtim.ToString() + "s.jpg";
            _imageUrl = @"http://i.4cdn.org/vg/" + _imageName;
            GetImage();

            _isValid = true;
        }

        string GetValue(string[] lines, string label)
        {
            foreach (var line in lines)
                if (line.Length > label.Length && line.Substring(0, label.Length) == label)
                {
                    return line.Substring(label.Length).Trim().Replace("https://", "");
                }
            return null;
        }

        void GetProgress(string[] lines)
        {
            GoodProgress = new List<string>();
            BadProgress = new List<string>();

            foreach (var line in lines)
                if (line.Length > 0)
                {
                    if (line[0] == '+')
                        GoodProgress.Add(line.Substring(1));
                    if (line[0] == '-')
                        GoodProgress.Add(line.Substring(1));
                }
        }

        public static AgdgEntry CreateEntry(JToken jObject, string postId)
        {
            var agdgEntry = new AgdgEntry(jObject, postId);

            if (agdgEntry._isValid)
                return agdgEntry;
            else
                return null;
        }

        public void GetImage()
        {
            var filePath = _postId + "\\" + _imageName;
            if (File.Exists(filePath))
                return;
            else
                using (var webClient = new System.Net.WebClient())
                { webClient.DownloadFile(_imageUrl, filePath); }
        }

        public XElement GetHtml()
        {
            XElement xDiv = new XElement("div");
            xDiv.Add(new XAttribute("class", "agdgEntry"));            
            
            xDiv.Add(CreateImageElement());
            xDiv.Add(CreateLabelElement());
            xDiv.Add(CreateTextElement());
            return xDiv;
        }

        XElement CreateLabelElement()
        {
            var xP = CreatePElement("", this.Game);
            xP.Add(new XAttribute("class", "agdgEntryTitle"));
            return xP;
        }

        XElement CreateTextElement()
        {
            XElement xDiv = new XElement("div");
            xDiv.Add(new XAttribute("class", "agdgEntryText"));
            
            xDiv.Add(CreateGameInfoElement());

            if (GoodProgress.Count> 0 || BadProgress.Count> 0)
                xDiv.Add(CreateProgressElement());

            return xDiv;
        }

        XElement CreateProgressElement()
        {
            XElement xDiv = new XElement("div");
            xDiv.Add(new XAttribute("class", "agdgEntryProgress"));

            foreach (var p in GoodProgress)
                xDiv.Add(CreatePElement(" + ", p));
            foreach (var p in BadProgress)
                xDiv.Add(CreatePElement(" - ", p));
            return xDiv;
        }

        XElement CreatePElement(string label, string value)
        {
            XElement xP = new XElement("p");
            var text = (label + value).Replace(">", "").Replace("<", "");
            xP.SetValue(text);
            return xP;
        }

        XElement CreateGameInfoElement()
        {
            XElement xDiv = new XElement("div");
            xDiv.Add(new XAttribute("class", "agdgEntryInfo"));
            xDiv.Add(CreatePElement(" Dev   : ", this.Dev));
            xDiv.Add(CreatePElement(" Web   : ", this.Web));
            xDiv.Add(CreatePElement(" Tools : ", this.Tools));
            return xDiv;
        }

        XElement CreateImageElement()
        {
            var filePath = _postId + "\\" + _imageName;
            XElement xI = new XElement("img");
            xI.Add(new XAttribute("src", filePath));

            XElement xD = new XElement("div");
            xD.Add(xI);
            xD.Add(new XAttribute("class", "agdgEntryImage"));
            
            return xD;
        }
    }
}
