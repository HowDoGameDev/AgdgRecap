namespace AgdgRecap
{
    class Program
    {
        static void Main(string[] args)
        {
            var entries = JsonAdapter.GetEntries(@"http://a.4cdn.org/vg/thread/142358482.json", "142364412", true);
            HtmlWriter.WriteHtml(entries);
        }
    }
}
