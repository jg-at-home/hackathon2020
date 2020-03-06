using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Hackathon2020
{
    public static partial class PostChecker
    {
        public static Status Run(ViewModel viewModel, string text)
        {
            var score = 0;

            score += checkUsers(viewModel, text);
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

        private static int checkUsers(ViewModel viewModel, string text)
        {
            var score = 0;
            var userMatches = Regex.Matches(text, @"\B@[A-Za-z]\w+\b");
            // Only count users once.
            var scoredUsers = new HashSet<string>();
            foreach (Match match in userMatches) {
                var userName = match.Value.Substring(1);
                if (!scoredUsers.Contains(userName)) {
                    scoredUsers.Add(userName);
                    var user = viewModel.Users.Find(u => u.UserName == userName);
                    if ((user != null) && (user.PostCount > 0)) {
                        score += (user.RedCount * 100) / user.PostCount;
                    }
                }
            }

            return score;
        }
    }
}
