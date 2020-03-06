using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Hackathon2020
{
    public static class Utils
    {
        public static void AddRange<T>(this ObservableCollection<T> list, IEnumerable<T> items)
        {
            foreach (var item in items) {
                list.Add(item);
            }
        }

        public static string QualityText(Status status)
        {
            switch (status) {
                case Status.Good:
                    return "This post appears to be free from Fake News";

                case Status.Dubious:
                    return "This post contains suspicious content";

                case Status.Deplorable:
                    return "Awooga! Awooga! Fake News alert!";

                default:
                    return "???";
            }
        }

        public static Brush QualityBrush(Status status)
        {
            switch (status) {
                case Status.Good:
                    return Brushes.Green;

                case Status.Dubious:
                    return Brushes.Orange;

                case Status.Deplorable:
                    return Brushes.Red;

                default:
                    return Brushes.BlueViolet;
            }
        }
    }
}
