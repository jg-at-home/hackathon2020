using System.Windows;
using System.Windows.Controls;

namespace Hackathon2020
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = (ViewModel) DataContext;
        }


        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox) {
                listBox.ScrollIntoView(listBox.SelectedItem);
            }
        }

        private ViewModel _viewModel;
    }
}
