using System;
using System.Text.RegularExpressions;
using System.Threading;

namespace Hackathon2020
{
    public static partial class PostChecker
    {
        public static Status Run(ViewModel viewModel, string text)
        {
            var score = 0;

            // 1. Find all references to users '@[letter|symbol][letter|symbol|number]*'

            var userMatches = Regex.Matches(text, @"\B@[A-Za-z]\w+\b");
            foreach (var match in userMatches) {

            }

            score += checkURLs(viewModel, text);

            if (score > 67) {
                return Status.Deplorable;
            }
            else if (score > 33) {
                return Status.Dubious;
            }
            else {
                return Status.Good;
            }
        }
    }
}
