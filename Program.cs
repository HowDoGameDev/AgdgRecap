using System.Collections.Generic;
namespace AgdgRecap
{
    class Program
    {
        static void Main(string[] args)
        {
            var entries = GetEntries("146019151", "146062906");
            entries.AddRange(GetEntries("146080779", "146062906"));
            HtmlWriter.WriteHtml(entries, "2016June4");

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
            foreach (var k in combos)
            {
                System.Console.WriteLine("(" + k.Value + ") " + k.Key);
            }
            System.Console.ReadKey();
        }

        static SortedDictionary<string, int> GetComboCounter(List<string> games)
        {
            var combos = new SortedDictionary<string, int>();

            List<string> recaps = new List<string>();
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
                games.Add(e.Game);
            }

            return games;
        }

        static List<AgdgEntry> GetEntries(string thread, string post)
        {
            return JsonAdapter.GetEntries(@"http://a.4cdn.org/vg/thread/" + thread + ".json", post, true);
        }

        static void CreateRecap()
        {
            //var entries = JsonAdapter.GetEntries(@"http://a.4cdn.org/vg/thread/142358482.json", "142364412", true);
            //var entries = JsonAdapter.GetEntries(@"http://a.4cdn.org/vg/thread/143095905.json", "143099264", true);
            var entries = new System.Collections.Generic.List<AgdgEntry>();
            var thread = "143833158";
            var post = "143875071";
            entries.AddRange(JsonAdapter.GetEntries(@"http://a.4cdn.org/vg/thread/" + thread + ".json", post, true));

            thread = "143903226";
            post = "143911548";
            entries.AddRange(JsonAdapter.GetEntries(@"http://a.4cdn.org/vg/thread/" + thread + ".json", post, true));

            //var thread = "143165860";
            //var post = "143166254";
            //entries = JsonAdapter.GetEntries(@"http://a.4cdn.org/vg/thread/" + thread + ".json", post, true);

            //thread = "143095905";
            //post = "143099264";
            //entries.AddRange(JsonAdapter.GetEntries(@"http://a.4cdn.org/vg/thread/" + thread + ".json", post, true));

            //thread = "143241095";
            //post = "143166254";            
            //entries.AddRange(JsonAdapter.GetEntries(@"http://a.4cdn.org/vg/thread/" + thread + ".json", post, true));

            HtmlWriter.WriteHtml(entries, "2016June1");
        }
    }
}
