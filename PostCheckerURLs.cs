using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using Helpers;

namespace Hackathon2020
{
    public static partial class PostChecker
    {
        private static int checkKnownDomains(ViewModel viewModel, string text)
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
                else {
                    result += processUrl(viewModel, domain);
                }
            }

            return result;
        }

        private static int processUrl(ViewModel viewModel, string url)
        {
            using (var webClient = new WebClient()) {
                var siteText = webClient.DownloadString(url);
                var lines = siteText.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
                var words = new List<string>();
                foreach (var line in lines) {
                    var lineWords = line.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries);
                    words.AddRange(lineWords);
                }

                var corpus = string.Join("\n", words.ToArray());
                foreach (var keyword in viewModel.Keywords) {
                    var regex = new Regex($@"\b{keyword}\b");
                    var matches = regex.Matches(corpus);
                    foreach (Match match in matches) {

                    }
                }
            }

            return 0;
        }
    }
}
