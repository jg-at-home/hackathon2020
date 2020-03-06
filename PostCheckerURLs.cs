using System.Text.RegularExpressions;
using Helpers;

namespace Hackathon2020
{
    public static partial class PostChecker
    {
        public static int checkKnownDomains(ViewModel viewModel, string text)
        {
            var result = 0;
            // Yes, a regex for a URL is a beast to behold.
            var regex = @"(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_]*)?";
            var matches = Regex.Matches(text, regex);
            foreach(Match match in matches) {
                var domain = UrlHelpers.ExtractDomainFromUrl(match.Value.ToLower());
                if (viewModel.KnownDomainScores.TryGetValue(domain, out var score)) {
                    result += score;
                }
            }

            return result;
        }
    }
}
