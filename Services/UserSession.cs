using ProdLogApp.Models;

namespace ProdLogApp.Services
{
    public class UserSession
    {
        private static UserSession _instance;
        public User ActiveUser { get; private set; }
     
        private UserSession() { }

        public static UserSession GetInstance()
        {
            if (_instance == null)
                _instance = new UserSession();
            return _instance;
        }

        public void SetUser(User user)
        {
            ActiveUser = user;
        }
    }
}
