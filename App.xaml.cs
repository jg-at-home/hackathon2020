using MVVM;

namespace Hackathon2020
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            DialogService.SetDialogService(new WindowedDialogService());
        }
    }
}
