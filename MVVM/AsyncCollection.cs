using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

namespace MVVM
{
    public class AsyncCollection<T> : ObservableCollection<T>
    {
        public AsyncCollection()
        {
            _context = SynchronizationContext.Current;
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (SynchronizationContext.Current == _context) {
                raiseCollectionChanged(e);
            }
            else {
                _context.Send(raiseCollectionChanged, e);
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (SynchronizationContext.Current == _context) {
                raisePropertyChanged(e);
            }
            else {
                _context.Send(raisePropertyChanged, e);
            }
        }

        private void raiseCollectionChanged(object arg)
        {
            base.OnCollectionChanged((NotifyCollectionChangedEventArgs)arg);
        }

        private void raisePropertyChanged(object arg)
        {
            base.OnPropertyChanged((PropertyChangedEventArgs)arg);
        }

        private readonly SynchronizationContext _context;
    }
}
