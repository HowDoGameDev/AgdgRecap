using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;

namespace AgdgRecap
{
    class JsonAdapter
    {
        public static List<AgdgEntry> GetEntries(string url, string postId, bool save = true)
        {
            List<AgdgEntry> entries = new List<AgdgEntry>();
            var threadId = System.IO.Path.GetFileNameWithoutExtension(url);
            var jsonFileName = postId + ".json";
            var jsonString = "";

            //Pull down jSon for thread
            //if (!System.IO.File.Exists(jsonFileName))
            //{
                using (var webClient = new System.Net.WebClient())
                { jsonString = webClient.DownloadString(url); }
                System.IO.File.WriteAllText(jsonFileName, jsonString);
            //}

            //Create directory for images.
            if (!System.IO.Directory.Exists(postId))
                System.IO.Directory.CreateDirectory(postId);            

            //Create entries. Some of this should probably be in the AgdgEntry.CreateEntry method.
            entries = new List<AgdgEntry>();
            //var targetId = String.Format("<a href=\"#p{0}\"", postId);
            var targetId = postId;
            jsonString = System.IO.File.ReadAllText(jsonFileName);
            JObject jO = JObject.Parse(jsonString);
            foreach (var jChild in jO.Children().First().First()) //Iterate through all posts.
            {
                var jv = (JValue)jChild["com"];
                if (jv != null && jv.ToString().Contains(targetId))
                {
                    var entry = AgdgEntry.CreateEntry(jChild, postId);                    
                    if (entry != null)
                        entries.Add(entry);
                }
            }

            return entries;
        }
    }
}
