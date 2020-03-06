using System.Windows;

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

        private ViewModel _viewModel;
    }
}
