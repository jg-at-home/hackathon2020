using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Input;
using System.Xml;
using MVVM;

namespace Hackathon2020
{
    class PostProxy
    {
        public PostProxy(int postID, int posterID, string body, Status status)
        {
            ResponseIDs = new List<int>();
            PostID = postID;
            PosterID = posterID;
            Body = body;
            Status = status;
            ParentID = -1;
        }

        public readonly int PostID;
        public readonly int PosterID;
        public readonly List<int> ResponseIDs;
        public readonly Status Status;
        public readonly string Body;
        public int ParentID;
    }

    public class ViewModel : ViewModelBase
    {
        public ViewModel()
        : base(null)
        {
            ComposeCommand = new RelayCommand((_)=>true, composePost);

            loadUsers();
            loadPosts();
            checkPosts();
        }

        public ICommand ComposeCommand { get; }

        public Post SelectedPost
        {
            get => _selectedPost;
            set {
                if (value != _selectedPost) {
                    _selectedPost = value;
                    OnPropertyChanged("SelectedPost");

                    Responses.Clear();
                    Responses.AddRange(_selectedPost.Responses);
                    OnPropertyChanged("SelectedResponses");
                }
            }
        }

        public int SelectedPostIndex
        {
            get => _selectedPostIndex;
            set {
                if (_selectedPostIndex != value) {
                    _selectedPostIndex = value;
                    OnPropertyChanged("SelectedPostIndex");
                    SelectedPost = Posts[_selectedPostIndex];
                }
            }
        }

        public int SelectedResponseIndex
        {
            get => -1;
            set {
                var post = Responses[value];
                Posts.Clear();
                Posts.Add(post);
                SelectedPost = post;
            } 
        }

        public ChitterUser ActiveUser
        {
            get => _currentUser;
            set {
                if (_currentUser != value) {
                    _currentUser = value;
                    OnPropertyChanged("ActiveUser");
                }
            }
        }

        public bool UnreadPosts
        {
            get => _unreadPosts;
            set {
                if (_unreadPosts != value) {
                    _unreadPosts = value;
                    OnPropertyChanged("UnreadPosts");
                }
            }
        }

        public ObservableCollection<Post> Posts { get; } = new ObservableCollection<Post>();

        public ObservableCollection<Post> Responses { get; } = new ObservableCollection<Post>();

        private void composePost(object arg)
        {
            var viewModel = new ComposeViewModel(_currentUser);
            DialogService.Instance.ShowDialog(viewModel);
            if (viewModel.Result) {
                var post = new Post(null, _nextPostID, _currentUser);
                ++_nextPostID;
                Posts.Add(post);
            }
        }

        private void loadUsers()
        {
            var doc = new XmlDocument();
            doc.Load("./Data/users.xml");
            var userNodes = doc.SelectNodes("/users/user");
            if (userNodes != null) {
                foreach (var node in userNodes) {
                    if (node is XmlElement userElement) {
                        var user = new ChitterUser(this) {
                            ID = int.Parse(userElement.GetAttribute("ID")),
                            Avatar = int.Parse(userElement.GetAttribute("avatar")),
                            RealName = userElement.GetAttribute("name"),
                            UserName = userElement.GetAttribute("username")
                        };
                        _users.Add(user);
                        Debug.WriteLine($"Chitter: added user '@{user.UserName}'");
                    }
                }
            }
        }

        private void loadPosts()
        {
            var doc = new XmlDocument();
            doc.Load("./Data/posts.xml");
            var postNodes = doc.SelectNodes("/posts/post");
            var proxies = new List<PostProxy>();
            var maxPostID = -1;
            if (postNodes != null) {
                foreach (var node in postNodes) {
                    if (node is XmlElement postElement) {
                        var postID = int.Parse(postElement.GetAttribute("ID"));
                        if (postID > maxPostID) {
                            maxPostID = postID;
                        }

                        var posterID = int.Parse(postElement.GetAttribute("user"));
                        var body = readPostBody(postElement);
                        var status = (Status) Enum.Parse(typeof(Status), postElement.GetAttribute("status"));
                        var postProxy = new PostProxy(postID, posterID, body, status);
                        readResponses(postElement, postProxy.ResponseIDs);
                        proxies.Add(postProxy);
                    }
                }
            }

            _nextPostID = maxPostID + 1;

            // Patch up parents.
            foreach(var postProxy in proxies) {
                foreach (var responseID in postProxy.ResponseIDs) {
                    var responseProxy = proxies.Find(p => p.PostID == responseID);
                    if (responseProxy != null) {
                        responseProxy.ParentID = postProxy.PostID;
                    }
                }
            }

            // Now generate the actual posts. First fix up the parents (needed because of the ViewModelBase ctor)...
            var postList = new List<Post>();
            foreach (var proxy in proxies) {
                Post parent = null;
                if (proxy.ParentID >= 0) {
                    parent = postList.Find(p => p.ID == proxy.ParentID);
                }

                var user = _users.Find(u => u.ID == proxy.PosterID);
                Debug.Assert(user != null);

                var post = new Post(parent, proxy.PostID, user) {BodyText = proxy.Body};

                // Only add root level posts to the visible post list. Child posts appear in the response panel.
                if (proxy.ParentID < 0) {
                    Posts.Add(post);    
                }

                postList.Add(post);

                // If quality not determined, add to the list to be checked later.
                if (post.Quality == Status.Unclassified) {
                    _postsToCheck.Add(post);
                }
            }

            // ...and the children.
            foreach (var proxy in proxies) {
                var post = postList.Find(p => p.ID == proxy.PostID);
                foreach (var responseID in proxy.ResponseIDs) {
                    var response = postList.Find(p => p.ID == responseID);
                    Debug.Assert(response != null);
                    post.Responses.Add(response);
                }
            }

            // Select the first post.
            if (Posts.Count > 0) {
                SelectedPost = Posts[0];
            }
        }

        private void checkPosts()
        {
            foreach (var post in _postsToCheck) {
                validatePost(post);
            }   
            
            _postsToCheck.Clear();
        }

        private void validatePost(Post post)
        {
            ThreadPool.QueueUserWorkItem(runPostChecks, post);
        }

        private static void runPostChecks(object arg)
        {
            var post = (Post) arg;
            var status = PostChecker.Run(post.BodyText);
            post.Quality = status;
            var user = post.User;
            ++user.PostCount;
            if (status == Status.Deplorable) {
                ++user.RedCount;
            }
        }

        private static void readResponses(XmlElement postElement, List<int> responses)
        {
            var responsesNode = postElement.SelectSingleNode("responses");
            if (responsesNode is XmlElement responsesElement) {
                var idList = responsesElement.GetAttribute("IDs");
                if (!string.IsNullOrEmpty(idList)) {
                    var idStrs = idList.Split(',');
                    foreach (var idStr in idStrs) {
                        responses.Add(int.Parse(idStr));
                    }
                }
            }
        }

        private static string readPostBody(XmlElement node)
        {
            var bodyNode = node.SelectSingleNode("body");
            if (bodyNode is XmlElement bodyElement) {
                return bodyElement.InnerText;
            }

            return string.Empty;
        }

        private Post _selectedPost;
        private int _selectedPostIndex;
        private bool _unreadPosts;
        private readonly List<ChitterUser> _users = new List<ChitterUser>();
        private ChitterUser _currentUser;
        private readonly List<Post> _postsToCheck = new List<Post>();
        private int _nextPostID;
    }
}
