using NewStage2VK.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewStage2VK.Presenter
{
    /// <summary>
    /// Интерфейс презентера для окна авторизации
    /// </summary>
    public interface ILoginPresenter
    {
        /// <summary>
        /// Отобразить окно авторизации
        /// </summary>
        void ShowLoginWindow();

        /// <summary>
        /// Событие, срабатывающее после завершения авторизации
        /// </summary>
        event EventHandler<LoginCompletedEventArgs> LoginCompleted;
    }
}
