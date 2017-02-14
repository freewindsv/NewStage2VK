using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NewStage2VK.View;
using NewStage2VK.DomainModel;
using NewStage2VK.ViewModel;

namespace NewStage2VK.Presenter
{
    /// <summary>
    /// Презентер для представления авторизации
    /// </summary>
    public class LoginPresenter : ILoginPresenter
    {
        private ILoginView loginView;
        private IVkDomainModel model;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="loginView">Представление авторизации</param>
        /// <param name="model">Модель приложения</param>
        public LoginPresenter(ILoginView loginView, IVkDomainModel model)
        {
            this.model = model;
            this.loginView = loginView;
            this.loginView.PageLoad += loginView_PageLoad;
        }

        #region View handlers

        /// <summary>
        /// Обработчик события представления загрузки страницы
        /// </summary>
        /// <param name="sender">Объект-отправитель события</param>
        /// <param name="e">Аргументы события окончания загрузки страницы браузером</param>
        private void loginView_PageLoad(object sender, PageLoadEventArgs e)
        {
            AuthResult result = GetAuthResult(e.Uri);
            if (result.Status == AuthStatus.Success)
            {
                Identity identity = new Identity()
                {
                    AccessToken = result.AccessToken,
                    UserId = result.UserId
                };
                if (result.ExpireIn > 0)
                {
                    identity.ExpireIn = TimeSpan.FromSeconds(result.ExpireIn);
                }
                model.LoggedUser = identity;
                loginView.Close();
                OnLoginCompleted(new LoginCompletedEventArgs(true));
            }
            else if (result.Status == AuthStatus.Error || result.Status == AuthStatus.AccessDenied)
            {
                loginView.Close();
                OnLoginCompleted(new LoginCompletedEventArgs(false));
            }
        }

        #endregion

        #region private members

        /// <summary>
        /// Вернуть результат авторизации
        /// </summary>
        /// <param name="uri">Uri, содержащий информацию о статусе авторизации</param>
        /// <returns>Результат выполнения авторизации</returns>
        private AuthResult GetAuthResult(Uri uri)
        {
            AuthResult result = new AuthResult() { Status = AuthStatus.Error };
            if (uri.Fragment.IndexOf("access_token") > -1)
            {
                string fragment = uri.Fragment;
                if (fragment.IndexOf("#") == 0)
                {
                    fragment = fragment.Substring(1);
                }
                try
                {
                    IDictionary<string, string> items = fragment.Split(new char[] { '&' }).Select(x =>
                    {
                        return x.Split(new char[] { '=' });

                    }).ToDictionary(x => x[0], x => x[1]);

                    result.AccessToken = items["access_token"];
                    result.UserId = int.Parse(items["user_id"]);
                    result.ExpireIn = int.Parse(items["expires_in"]);
                    result.Status = AuthStatus.Success;
                }
                catch (Exception)
                {
                    result.Status = AuthStatus.Error;
                }
            }
            else if (uri.AbsoluteUri.IndexOf("authorize") > -1)
            {
                if (uri.AbsoluteUri.IndexOf("q_hash") > -1)
                {
                    result.Status = AuthStatus.RequestPermissions;
                }
                else if (uri.AbsoluteUri.IndexOf("email") > -1)
                {
                    result.Status = AuthStatus.WrongPassword;
                }
                else
                {
                    result.Status = AuthStatus.Init;
                }
            }
            else if (uri.AbsolutePath.IndexOf("access_denied") > -1)
            {
                result.Status = AuthStatus.AccessDenied;
            }
            return result;
        }

        #endregion

        #region ILoginPresenter implementation

        /// <summary>
        /// Отобразить окно авторизации
        /// </summary>
        public void ShowLoginWindow()
        {
            loginView.OpenLoginPage(model.LoginUrl);
        }

        /// <summary>
        /// Событие, срабатывающее после завершения авторизации
        /// </summary>
        public event EventHandler<LoginCompletedEventArgs> LoginCompleted;

        /// <summary>
        /// Метод, запускающий событие
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnLoginCompleted(LoginCompletedEventArgs args)
        {
            if (LoginCompleted != null)
            {
                LoginCompleted(this, args);
            }
        }

        #endregion
    }
}
