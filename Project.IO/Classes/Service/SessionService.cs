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

        private int? _userId;
        private bool _isLoggedIn;
        private int? _projectId;

        private SessionService()
        {
            _userId = null;
            _isLoggedIn = false;
            _projectId = null;
        }

        public int? UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { _isLoggedIn = value; }
        }

        public int? ProjectId
        {
            get { return _projectId; }
            set { _projectId = value; }
        }

        public void Logout()
        {
            UserId = null;
            IsLoggedIn = false;
        }
    }
}