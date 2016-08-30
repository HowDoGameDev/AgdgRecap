using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace AgdgRecap
{
    class JsonAdapter
    {
        public static List<AgdgEntry> GetEntries(string url, string threadId, string targetId)
        {
            List<string> blackListGames = new List<string>();
            blackListGames.Add("butt");
            blackListGames.Add("nothing");
            blackListGames.Add("all of you are whodevs");
            var jsonString = "";
            var jsonFileName = threadId + ".json";
            using (var webClient = new System.Net.WebClient())
            { jsonString = webClient.DownloadString(url); }
            System.IO.File.WriteAllText(jsonFileName, jsonString);

            if (!System.IO.Directory.Exists(threadId))
                System.IO.Directory.CreateDirectory(threadId);

            List<AgdgEntry> entries = new List<AgdgEntry>();
            jsonString = System.IO.File.ReadAllText(jsonFileName);
            JObject jO = JObject.Parse(jsonString);
            foreach (var jChild in jO.Children().First().First()) //Iterate through all posts.
            {
                var jv = (JValue)jChild["com"];
                if (jv != null && (jv.ToString().Contains("==|AGDG Weekly Recap|==") || jv.ToString().Contains(targetId)))
                {
                    var entry = AgdgEntry.CreateEntry(jChild, threadId);
                    if (entry != null)
                    {
                        if (blackListGames.Contains(entry.Game.ToLower()))
                            continue;
                        entries.Add(entry);
                    }
                }
            }

            return entries;
        }

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
    
        public static List<string> GetAgdgThreads(DateTime date)
        {
            var startDate = StartOfWeek(date, DayOfWeek.Monday);
            List<string> agdgThreads = new List<string>();

            var url = @"https://a.4cdn.org/vg/archive.json";
            //var url = @"http://boards.4chan.org/vg/archive";
            var jsonString = "";
            var jsonFileName = "jsonArchive.json";
            using (var webClient = new System.Net.WebClient())
            { jsonString = webClient.DownloadString(url); }
            //System.IO.File.WriteAllText(jsonFileName, jsonString);

            //XDocument xDoc = XDocument.Load(jsonFileName);
            //var xAgdgThreads = xDoc.Descendants("td").Where(x => x.Attribute("class") != null &&
            //    x.Attribute("class").Value == "teaser-col" &&
            //    x.Value.Substring(0, 6) == "/agdg/").ToList();

            var threads = jsonString.Substring(1, jsonString.Length - 2).Split(',').Reverse();

            bool killCheck = false;
            foreach (var thread in threads)           
            {
                if (IsAgdgThread(date, thread, out killCheck))
                    agdgThreads.Add(thread);
                if (killCheck)
                    break;
            }
            return agdgThreads;
        }

        static bool IsAgdgThread(DateTime date, string threadId, out bool killCheck)
        {
            killCheck = false;
            var jsonString = "";
            var url = "http://a.4cdn.org/vg/thread/" + threadId + ".json";
            using (var webClient = new System.Net.WebClient())
            { jsonString = webClient.DownloadString(url); }
            
            JObject jO = JObject.Parse(jsonString);
            var jChild = jO.Children().First().First().First();
            var derp = DateTime.Parse(((JValue)jChild["now"]).Value.ToString().Substring(0, 8));

            if (derp >= date)
            {
                Console.WriteLine();
                var jv = ((JValue)jChild["sub"]).Value.ToString();

                return jv.Contains("agdg");
            }
            else if ((derp - date).Days > 2)
                killCheck = true;

            return false;
        }

        public static DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(-1 * diff).Date;
        }
        
    }
}
