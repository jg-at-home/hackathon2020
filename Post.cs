using System.Collections.Generic;
using MVVM;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Hackathon2020
{
    public enum Status
    {
        Unclassified,
        Good,
        Dubious,
        Deplorable
    }
    
    public class Post : ViewModelBase
    {
        public Post(Post parent, int id, ChitterUser user) 
        : base(parent)
        {
            User = user;
            AvatarIndex = user.Avatar;
            ID = id;
        }

        public readonly int ID;

        public string BodyText
        {
            get => _bodyText;
            set {
                if (value != _bodyText) {
                    _bodyText = value;
                    OnPropertyChanged("BodyText");
                }
            }
        }

        public string Avatar => $"/Hackathon2020;component/Images/avatar{_avatarIndex+1}.png";

        public int AvatarIndex
        {
            get => _avatarIndex;
            set {
                if (value != _avatarIndex) {
                    _avatarIndex = value;
                    OnPropertyChanged("Avatar");
                }
            }
        }

        public Status Quality
        {
            get => _status;
            set {
                if (value != _status) {
                    _status = value;
                    OnPropertyChanged("QualityText");
                    OnPropertyChanged("QualityBrush");
                }
            }
        }

        public string QualityText => Utils.QualityText(_status);

        public Brush QualityBrush => Utils.QualityBrush(_status);

        public string UserName => User.UserName;

        public ChitterUser User { get; }

        public int UserRedCount => User.RedCount;
        public int UserPostCount => User.PostCount;

        public readonly List<Post> Responses = new List<Post>();

        private int _avatarIndex;
        private string _bodyText;
        private Status _status;
    }
}
