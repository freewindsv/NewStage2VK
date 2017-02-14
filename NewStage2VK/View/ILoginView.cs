using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NewStage2VK.ViewModel;

namespace NewStage2VK.View
{
    /// <summary>
    /// Интерфейс представления авторизации оператора
    /// </summary>
    public interface ILoginView
    {
        /// <summary>
        /// Открыть страницу авторизации ВК
        /// </summary>
        /// <param name="url">Url страницы авторизации</param>
        void OpenLoginPage(string url);

        /// <summary>
        /// Событие, срабатывающее после окончания загрузки страницы
        /// </summary>
        event EventHandler<PageLoadEventArgs> PageLoad;

        /// <summary>
        /// Закрыть представление (окно) авторизации
        /// </summary>
        void Close();
    }
}
