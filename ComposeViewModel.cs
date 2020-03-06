using System;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MVVM;

namespace Hackathon2020
{
    public class ComposeViewModel : DialogViewModel
    { 
        public ComposeViewModel(ViewModel viewModel, ChitterUser poster) 
        : base("Compose post")
        {
            PostCommand = new RelayCommand(canPost, onPost);
            CancelCommand = new RelayCommand((x)=>true, onCancel);
            _poster = poster;
            _viewModel = viewModel;
            Quality = Status.Unclassified;
        }

        public bool Result { get; private set; }

        public string Message
        {
            get => _messageText;
            set {
                if (_messageText != value) {
                    _messageText = value;
                    OnPropertyChanged("Message");
                    if (!string.IsNullOrEmpty(value)) {
                        waitForIdle();
                    }
                }
            }
        }

        public Status Quality
        {
            get => _status;
            set {
                if (_status != value) {
                    _status = value;
                    OnPropertyChanged("QualityText");
                    OnPropertyChanged("QualityBrush");
                }
            }
        }

        public string QualityText => Utils.QualityText(_status);

        public Brush QualityBrush => Utils.QualityBrush(_status);

        public ICommand PostCommand { get; }

        public ICommand CancelCommand { get; }

        private void waitForIdle()
        {
            if (_idleTimer == null) {
                _idleTimer = new DispatcherTimer();
                _idleTimer.Interval = TimeSpan.FromMilliseconds(500);
                _idleTimer.Tick += idleTimerOnTick;
            }
            _idleTimer.Stop();
            _idleTimer.Start();
        }

        private void idleTimerOnTick(object sender, EventArgs e)
        {
            _idleTimer.Stop();
            if (!string.IsNullOrEmpty(_messageText)) {
                ThreadPool.QueueUserWorkItem((_) => { Quality = PostChecker.Run(_viewModel, _messageText); });
            }
        }

        private bool canPost(object arg)
        {
            return !string.IsNullOrEmpty(_messageText);
        }

        private void onPost(object arg)
        {
            Result = true;
            ++_poster.PostCount;
            if (_status == Status.Deplorable) {
                ++_poster.RedCount;
            }
            CloseDialog();
        }

        private void onCancel(object arg)
        {
            Result = false;
            CloseDialog();
        }

        private string _messageText;
        private Status _status;
        private readonly ChitterUser _poster;
        private DispatcherTimer _idleTimer;
        private readonly ViewModel _viewModel;
    }
}
