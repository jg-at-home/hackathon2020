using System.ComponentModel;

namespace MVVM
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public ViewModelBase(ViewModelBase parent)
        {
            Parent = parent;
        }

        public ViewModelBase Parent { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
            Parent?.OnChildPropertyChanged(this, propName);
        }

        protected virtual void OnChildPropertyChanged(ViewModelBase child, string property)
        {

        }
    }
}
