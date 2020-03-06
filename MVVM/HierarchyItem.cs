using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MVVM
{
    public class HierarchyItem : ViewModelBase
    {
        public HierarchyItem(HierarchyItem parent, string text, object tag)
        : base(parent)
        {
            Tag = tag;
            Text = text;
        }

        public object Tag { get; }
        public string Text { get; }
        public List<HierarchyItem> Children { get; } = new List<HierarchyItem>();
    }
}
