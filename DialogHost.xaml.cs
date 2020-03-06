using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MVVM;

namespace Hackathon2020
{
    /// <summary>
    /// Interaction logic for DialogHost.xaml
    /// </summary>
    public partial class DialogHost : IClosable
    {
        public DialogHost(Window parent, DialogViewModel viewModel)
        {
            Owner = parent;
            InitializeComponent();
            DataContext = viewModel;
            viewModel.CloseRequestEvent += Close;
        }

        private void DialogHost_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) {
                DragMove();
            }
        }

    }
}
