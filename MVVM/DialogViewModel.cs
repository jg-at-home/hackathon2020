using System.Windows.Input;

namespace MVVM
{
    public delegate void CloseRequest();

    public class DialogViewModel : ViewModelBase
    {
        public DialogViewModel(string caption) 
        : base(null)
        {
            Caption = caption;
        }

        public string Caption
        {
            get => _caption;
            set {
                if (_caption != value) {
                    _caption = value;
                    OnPropertyChanged("Caption");
                }
            }
        }

        public void CloseDialog()
        {
            CloseRequestEvent?.Invoke();
        }

        public event CloseRequest CloseRequestEvent;

        private string _caption;
    }
}
