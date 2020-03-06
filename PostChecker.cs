using System;
using System.Threading;

namespace Hackathon2020
{
    public static class PostChecker
    {
        public static Status Run(string text)
        {
            // TODO.
            Thread.Sleep(s_rng.Next(1000, 4000));
            return (Status) s_rng.Next(1, 4);
        }

        static readonly Random s_rng = new Random(Environment.TickCount);
    }
}
