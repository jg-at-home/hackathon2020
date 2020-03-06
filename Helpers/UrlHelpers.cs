using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class UrlHelpers
    {
        public static string ExtractDomainFromUrl(string url)
        {
            var uri = new Uri(url);
            return uri.Host;
        }
    }
}
