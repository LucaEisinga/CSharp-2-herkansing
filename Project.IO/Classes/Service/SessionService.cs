using Blazored.LocalStorage;

namespace Project.IO.Classes.Service
{
    public class SessionService
    {

        private static SessionService _instance;
        private static readonly object _lock = new object();

        public static SessionService Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new SessionService();
                }
            }
        }

        private string? _userId;
        private bool _isLoggedIn;

        private SessionService()
        {
            _userId = null;
            _isLoggedIn = false;
        }

        public string? UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { _isLoggedIn = value; }
        }


    }
}
