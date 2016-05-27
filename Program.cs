namespace AgdgRecap
{
    class Program
    {
        static void Main(string[] args)
        {
            //var entries = JsonAdapter.GetEntries(@"http://a.4cdn.org/vg/thread/142358482.json", "142364412", true);
            //var entries = JsonAdapter.GetEntries(@"http://a.4cdn.org/vg/thread/143095905.json", "143099264", true);
            var entries = new System.Collections.Generic.List<AgdgEntry>();

            var thread = "143165860";
            var post = "143166254";
            entries = JsonAdapter.GetEntries(@"http://a.4cdn.org/vg/thread/" + thread + ".json", post, true);

            thread = "143095905";
            post = "143099264";
            entries.AddRange(JsonAdapter.GetEntries(@"http://a.4cdn.org/vg/thread/" + thread + ".json", post, true));

            thread = "143241095";
            post = "143166254";            
            entries.AddRange(JsonAdapter.GetEntries(@"http://a.4cdn.org/vg/thread/" + thread + ".json", post, true));
            
            HtmlWriter.WriteHtml(entries);
        }
    }
}
