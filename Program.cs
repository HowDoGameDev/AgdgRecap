using System.Collections.Generic;
namespace AgdgRecap
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateAgdgRecap();
            System.Console.ReadLine();
        }

        static void CreateAgdgRecap()
        {
            //Get Entries
            var entries = new List<AgdgEntry>();
            List<string> threadIds = new List<string>() 
            {
                "153267796",
            };
            foreach (var tIds in threadIds)
                entries.AddRange(GetEntries(tIds, ""));

            //Generate HTML
            HtmlWriter.WriteHtml(entries, "2016Aug5", "August 2016 Week 5");

            //Display Combos
            CalculateCombos(entries);
        }

        static void CalculateCombos(List<AgdgEntry> entries)
        {
            var games = new List<string>();
            foreach (var e in entries)
            {
                if (e.Game.Contains("Knightly Terrors"))
                    e.Game = "Knightly Terrors";

                if (!games.Contains(e.Game))
                    games.Add(e.Game);
            }
            System.Console.WriteLine(games.Count);

            var combos = GetComboCounter(games);

            System.Console.WriteLine();
            List<string> derp = new List<string>();
            foreach (var k in combos)
                derp.Add("(" + k.Value + ") " + k.Key);
            derp.Sort();
            derp.Reverse();
            foreach (var d in derp)
                System.Console.WriteLine(d);
        }

        static SortedDictionary<string, int> GetComboCounter(List<string> games)
        {
            var combos = new SortedDictionary<string, int>();

            List<string> recaps = new List<string>();
            recaps.Add("2016Aug3");
            recaps.Add("2016Aug2");
            recaps.Add("2016Aug1");
            recaps.Add("2016July4");
            recaps.Add("2016July3");
            recaps.Add("2016July2");
            recaps.Add("2016July1");
            recaps.Add("2016June5");
            recaps.Add("2016June4");
            recaps.Add("2016June3");
            recaps.Add("2016June2");
            recaps.Add("2016June1");
            recaps.Add("2016May4");

            foreach (var g in games)
            {
                int cnt = 1;
                foreach (var r in recaps)
                    if (!GetGamesForRecap(r).Contains(g))
                        break;
                    else
                        cnt++;

                if (cnt > 1)
                    combos.Add(g, cnt);
            }
            return combos;
        }

        static List<string> GetGamesForRecap(string recap)
        {
            var entries = HtmlAdapter.GetEntries(@"C:\Users\phillip\Documents\GameResearch\Recap\Recaps\" + recap + @"\" + recap + ".html");

            var games = new List<string>();
            foreach (var e in entries)
            {
                if (e.Game.Contains("Knightly Terrors"))
                    e.Game = "Knightly Terrors";

                if (e.Game.ToLower() == "unnamed pixel platformer")
                    e.Game = "Unnamed Pixel Platformer";

                games.Add(e.Game);
            }

            return games;
        }

        static List<AgdgEntry> GetEntries(string thread, string post)
        {
            //return JsonAdapter.GetEntries(@"http://a.4cdn.org/vg/thread/" + thread + ".json", post, true);
            return JsonAdapter.GetEntries(@"http://a.4cdn.org/vg/thread/" + thread + ".json", thread, post);
        }

        static void PrintMissingGames()
        {
            List<string> recaps = new List<string>();
            recaps.Add("2016Aug3");
            recaps.Add("2016Aug2");
            recaps.Add("2016Aug1");
            recaps.Add("2016July4");
            recaps.Add("2016July3");
            recaps.Add("2016July2");
            recaps.Add("2016July1");
            recaps.Add("2016June5");
            recaps.Add("2016June4");
            recaps.Add("2016June3");
            recaps.Add("2016June2");
            recaps.Add("2016June1");
            recaps.Add("2016May4");

            Dictionary<string, int> count = new Dictionary<string, int>();
            
            int weekCount = 0;
            foreach (var r in recaps)
            {
                foreach (var g in GetGamesForRecap(r))
                {
                    var game = g.ToLower().Trim();
                    if (count.ContainsKey(game))
                        continue;
                    else
                    {
                        count.Add(game, weekCount);
                    }
                }
                weekCount++;
            }

            var entries = new List<AgdgEntry>();
            foreach (var r in recaps)
            {
                foreach (var g in HtmlAdapter.GetEntries(r, false))
                {
                    if (g.Game.Contains("Knightly Terrors"))
                        g.Game = "Knightly Terrors";

                    if (g.Game.ToLower() == "unnamed pixel platformer")
                        g.Game = "Unnamed Pixel Platformer";

                    var game = g.Game.ToLower().Trim();


                    var c = count[game];

                    if (c > 2)
                    {
                        g.Game += " - " + c + " weeks";
                        entries.Add(g);
                        g.CopyImage();
                    }
                }
            }

            HtmlWriter.WriteHtml(entries, "MIA");

            System.Console.WriteLine();
        }
    }
}
