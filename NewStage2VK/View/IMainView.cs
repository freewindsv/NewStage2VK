using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NewStage2VK.DomainModel;

namespace NewStage2VK.View
{
    /// <summary>
    /// Интерфейс основного представления
    /// </summary>
    public interface IMainView : IStatusView
    {
        /// <summary>
        /// Событие окончания загрузки представления
        /// </summary>
        event EventHandler ViewLoaded;

        /// <summary>
        /// Событие, срабатывающее когда оператор инициировал выход из системы
        /// </summary>
        event EventHandler UserExit;

        /// <summary>
        /// Отобразить рисунок-заглушку на время загрузки аватара
        /// </summary>
        void ShowLoadingAvatar();

        /// <summary>
        /// Отобразить сообщение об ошибки доступа
        /// </summary>
        /// <param name="message">Сообщение об ошибки доступа</param>
        void ShowAccessFailMessage(string message);

        /// <summary>
        /// Очистить данные авторизованного оператора
        /// </summary>
        void ClearLoggedUser();

        /// <summary>
        /// Отобразить данные авторизованного оператора
        /// </summary>
        /// <param name="name">Имя оператора</param>
        /// <param name="imageBytes">Изображение аватарки</param>
        void ShowLoggedUser(string name, byte[] imageBytes);

        /// <summary>
        /// Закрыть представление (окно)
        /// </summary>
        void Close();

        /// <summary>
        /// Отключить все возможные действия
        /// </summary>
        void DisableAllActions();

        /// <summary>
        /// Включить все возможные действия
        /// </summary>
        void EnableAllActions();
    }
}
