using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewStage2VK.DomainModel
{
    public class LoginEventArgs : EventArgs
    {
        public LoginEventArgs(string url)
        {
            Url = url;
        }

        public string Url { get; private set; }
    }

    public class VkDomainModel : IVkDomainModel
    {
        private const string LOGIN_URL = "https://oauth.vk.com/authorize?client_id=5794488&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope=messages,offline&response_type=token&v=5.60";

        public void TryLogin()
        {

        }

        public void SetLoggedUser(string accessToken, int userId)
        {
 
        }

        public event EventHandler<LoginEventArgs> OnTryLogin;

        protected virtual void fireOnTryLogin()
        {
            if (OnTryLogin != null)
            {
                OnTryLogin(this, new LoginEventArgs(LOGIN_URL));
            }
        }
    }
}
