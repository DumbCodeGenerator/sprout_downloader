using System.Linq.Expressions;
using System.Reflection;

namespace Sprout_Downloader.Util
{
    public static class Extensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }

        public static bool CheckUrlValid(this string source)
        {
            return Uri.TryCreate(source, UriKind.Absolute, out Uri uriResult) &&
                   (uriResult.Scheme == Uri.UriSchemeHttps || uriResult.Scheme == Uri.UriSchemeHttp);
        }

        public static List<ParsedURL> ParseURL(this TextBox @this)
        {
            List<ParsedURL> urls = new List<ParsedURL>();
            foreach (string line in @this.Lines)
            {
                string[] parts = line.Split("||");
                if (parts.Length > 0)
                {
                    string url = parts[0];
                    if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                        url = "https://" + url;

                    string pass = null;
                    if (parts.Length >= 2)
                        pass = parts[1];

                    if (url.CheckUrlValid())
                        urls.Add(new ParsedURL { URL = url, Password = pass });
                }
            }
            return urls;
        }

        public static void SetPropertyThreadSafe<TResult>(
            this Control @this,
            Expression<Func<TResult>> property,
            TResult value)
        {
            PropertyInfo propertyInfo = (property.Body as MemberExpression)?.Member
                as PropertyInfo;

            if (propertyInfo == null ||
                //!@this.GetType().IsSubclassOf(propertyInfo.ReflectedType ?? throw new InvalidOperationException()) ||
                @this.GetType().GetProperty(
                    propertyInfo.Name,
                    propertyInfo.PropertyType) == null)
                throw new ArgumentException(
                    "The lambda expression 'property' must reference a valid property on this Control.");

            if (@this.InvokeRequired)
                @this.Invoke(new SetPropertyThreadSafeDelegate<TResult>
                    (SetPropertyThreadSafe), @this, property, value);
            else
                @this.GetType().InvokeMember(
                    propertyInfo.Name,
                    BindingFlags.SetProperty,
                    null,
                    @this,
                    new object[] { value });
        }

        private delegate void SetPropertyThreadSafeDelegate<TResult>(
            Control @this,
            Expression<Func<TResult>> property,
            TResult value);
    }
}