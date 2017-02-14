using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NewStage2VK.View;
using NewStage2VK.DomainModel;
using NewStage2VK.DataAccess.DataModel;
using NewStage2VK.DomainModel.Ext;
using NewStage2VK.Helpers;
using NewStage2VK.DomainModel.Crypto;
using NewStage2VK.ViewModel;

namespace NewStage2VK.Presenter
{
    /// <summary>
    /// Презентер для представления основной функциональности окна 
    /// </summary>
    public class MainPresenter
    {
        private IMainView mainView;
        private IVkDomainModel model;
        private ILoginPresenter loginPresenter;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="mainView">Представление базовой функциональности окна</param>
        /// <param name="model">Модель</param>
        /// <param name="loginPresenter">Объект конфигурации</param>
        public MainPresenter(IMainView mainView, IVkDomainModel model, ILoginPresenter loginPresenter)
        {
            this.mainView = mainView;
            this.model = model;
            this.loginPresenter = loginPresenter;

            this.loginPresenter.LoginCompleted += loginPresenter_LoginCompleted;

            this.mainView.ViewLoaded += mainView_ViewLoaded;
            this.mainView.UserExit += mainView_UserExit;
        }

        /// <summary>
        /// Обработчик события загрузки представления
        /// </summary>
        /// <param name="sender">Объект-отправитель события</param>
        /// <param name="e">Пустой объект аргументов события</param>
        private async void mainView_ViewLoaded(object sender, EventArgs e)
        {
            Settings settings = await model.DataAccess.SettingsRepository.GetCurrentSettingsAsync();
            if (settings == null || settings.CurrentUser == null)
            {
                DeleteAuthCookie();
                loginPresenter.ShowLoginWindow();
            }
            else
            {
                User user = settings.CurrentUser;
                try
                {
                    model.LoggedUser = user.GetIdentity(model.CryptoProvider);
                }
                catch (CryptoProviderException)
                {
                    loginPresenter.ShowLoginWindow();
                    return;
                }
                mainView.ShowLoggedUser(user.Name, user.Avatar_50);
                mainView.SetStatusText("Готово");
            }
        }

        /// <summary>
        /// Обработчик события выхода из приложения
        /// </summary>
        /// <param name="sender">Объект-отправитель события</param>
        /// <param name="e">Пустой объект аргументов события</param>
        private async void mainView_UserExit(object sender, EventArgs e)
        {
            Settings settings = await model.DataAccess.SettingsRepository.GetCurrentSettingsAsync();
            if (settings != null)
            {
                settings.CurrentUser = null;
                await model.DataAccess.SettingsRepository.SaveAsync();
            }
            mainView.ClearLoggedUser();
            DeleteAuthCookie();
            loginPresenter.ShowLoginWindow();
        }

        /// <summary>
        /// Обработчик события окончания авторизации
        /// </summary>
        /// <param name="sender">Объект-отправитель события</param>
        /// <param name="e">Аргументы события окончания авторизации</param>
        private async void loginPresenter_LoginCompleted(object sender, LoginCompletedEventArgs e)
        {
            if (e.Success)
            {
                try
                {
                    mainView.DisableAllActions();
                    mainView.SetStatusText("Загрузка информации о пользователе...");
                    mainView.ShowLoadingAvatar();

                    VkResult<VkUser> result = await model.GetUserInfoAsync(model.LoggedUser.UserId);
                    if (result.Error != null)
                    {
                        mainView.ShowError("Ошибка загрузки информации о пользователе. Приложение будет закрыто");
                        mainView.Close();
                        return;
                    }
                    byte[] imageBytes = await model.GetImageAsync(result.Value.AvatarUrl_50);
                    mainView.ShowLoggedUser(result.Value.GetName(), imageBytes);

                    mainView.SetStatusText("Сохранение аккаунта в базе данных");
                    User user = await model.DataAccess.UserRepository.GetByVkIdAsync(result.Value.Id);
                    if (user == null)
                    {
                        user = new User();
                    }
                    result.Value.UpdateUser(user, model.CryptoProvider.Crypt(model.LoggedUser.AccessToken), model.LoggedUser.ExpireIn, imageBytes);

                    Settings settings = await model.DataAccess.SettingsRepository.GetCurrentSettingsAsync();
                    if (settings == null)
                    {
                        model.DataAccess.SettingsRepository.Create(new Settings() { CurrentUser = user });
                    }
                    else
                    {
                        settings.CurrentUser = user;
                    }
                    await model.DataAccess.SettingsRepository.SaveAsync();

                    mainView.EnableAllActions();
                    mainView.SetStatusText("Готово");
                }
                catch
                {
                    mainView.ShowError("Произошла ошибка при инициализации авторизованного пользователя. Программа будет закрыта");
                    mainView.Close();
                }
            }
            else
            {
                mainView.ShowAccessFailMessage("Произошла ошибка доступа. Видимо, вы не разрешили программе доступ к данным, поэтому программа будет закрыта (");
                mainView.Close();
            }
        }

        /// <summary>
        /// Удалить авторизационные куки
        /// </summary>
        private void DeleteAuthCookie()
        {
            BrowserHelper.SuppressCookiePersistence();
        }
    }
}
