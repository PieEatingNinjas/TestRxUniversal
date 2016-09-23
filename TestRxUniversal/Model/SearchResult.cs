namespace TestRxUniversal.Model
{

    public class SearchResultRootobject
    {
        public int count { get; set; }
        public Source[] sources { get; set; }
    }

    public class Source
    {
        public string source_name { get; set; }
        public int count { get; set; }
        public Result[] result { get; set; }
        public object _params { get; set; }
    }

    public class Result
    {
        public string title { get; set; }
        public string description { get; set; }
        public string tags { get; set; }
        public string thumb_url { get; set; }
        public string preview_url { get; set; }
        public string url { get; set; }
        public string page_url { get; set; }
        public object thumb_width { get; set; }
        public object thumb_height { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string license { get; set; }
        public string author { get; set; }
    }

}
