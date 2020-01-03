using System.Collections.Specialized;
using System.Linq;

namespace Im.Access.Portal.Services
{
    public static class StringExtensions
    {
        public static bool IsLocalUrl(this string url)
        {
            return url.StartsWith('/');
        }

        public static string RemoveLeadingSlash(this string url)
        {
            return url.StartsWith('/') ? url.Substring(1) : url;
        }

        public static string EnsureTrailingSlash(this string url)
        {
            return url.EndsWith('/') ? url : $"{url}/";
        }

        public static string AddQueryString(this string url, string key, string value)
        {
            return url.AddQueryString($"{key}={value}");
        }

        public static string AddQueryString(this string url, string query)
        {
            return url.Contains('?') ? $"{url}&{query}" : $"{url}?{query}";
        }

        public static string ToQueryString(this NameValueCollection parameters)
        {
            return string.Join('&', parameters
                .AllKeys
                .SelectMany(
                    key =>
                    {
                        var values = parameters.GetValues((string) key);
                        return values.Select(value => $"{key}={value}");
                    }));
        }
    }
}