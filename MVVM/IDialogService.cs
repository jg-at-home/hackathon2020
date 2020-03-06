using System;

namespace MVVM
{
    public enum FileOperation
    {
        Load,
        Save
    };

    /// <summary>
    /// Interface for dialog service.
    /// </summary>
    public interface IDialogService
    {
        string GetFileFromFileSystemDialog(string caption, FileOperation operation, string filter);
        string GetFolderFromFileSystemDialog(string caption);
        void ShowMessage(string caption, string message);
        void ShowDialog(DialogViewModel dialogViewModel);
    }

    public static class DialogService
    {
        public static void SetDialogService(IDialogService dialogService)
        {
            if (Instance != null) {
                throw new ApplicationException("Dialog service already set");
            }

            Instance = dialogService;
        }

        public static IDialogService Instance { get; private set; }
    }
}