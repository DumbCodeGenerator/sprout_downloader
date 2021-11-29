namespace Sprout_Downloader.Util
{
    public class ParsedURL
    {
        public string URL { get; set; }
        public string Password { get; set; }

        public ParsedURL SetURL(string url)
        {
            URL = url;
            return this;
        }

        public ParsedURL SetPassword(string pass)
        {
            Password = pass;
            return this;
        }
    }
}
