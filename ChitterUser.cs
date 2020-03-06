using MVVM;

namespace Hackathon2020
{
    public class ChitterUser : ViewModelBase
    {
        public ChitterUser(ViewModel parent)
        : base(parent)
        {
        }

        public int ID { get; set; }
        public string RealName { get; set; }
        public string UserName { get; set; }
        public int Avatar { get; set; }

        public int RedCount
        {
            get => _redCount;
            set {
                if (_redCount != value) {
                    _redCount = value;
                    OnPropertyChanged("RedCount");
                }
            }
        }

        public int PostCount
        {
            get => _postCount;
            set {
                if (_postCount != value) {
                    _postCount = value;
                    OnPropertyChanged("PostCount");
                }
            }
        }

        private int _postCount;
        private int _redCount;
    }
}
