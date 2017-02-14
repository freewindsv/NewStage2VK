using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewStage2VK.DomainModel
{
    public interface IVkDomainModel
    {
        void TryLogin();
        event EventHandler<LoginEventArgs> OnTryLogin;
        void SetLoggedUser(string accessToken, int userId);
    }
}
