using System.Windows.Media.Effects;
using MVVM;
using Application = System.Windows.Application;

namespace Hackathon2020
{
    public class WindowedDialogService : IDialogService
    {
        public string GetFileFromFileSystemDialog(string caption, FileOperation operation, string filter)
        {
            return string.Empty;
        }

        public string GetFolderFromFileSystemDialog(string caption)
        {
            return string.Empty;
        }

        public void ShowMessage(string caption, string message)
        {
            // Not needed yet.
        }

        public void ShowDialog(DialogViewModel viewModel)
        {
            var parent = Application.Current.MainWindow;
            var dialog = new DialogHost(parent, viewModel);
            // ReSharper disable once PossibleNullReferenceException
            parent.Effect = new BlurEffect()
            {
                Radius = 4.0
            };
            try {
                dialog.ShowDialog();
            }
            finally {
                parent.Effect = null;
            }
        }
    }
}
