using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NewStage2VK.DataAccess.DataModel;
using NewStage2VK.ViewModel;

namespace NewStage2VK.View
{
    /// <summary>
    /// Интерфейс представления по рассылке сообщений
    /// </summary>
    /// <typeparam name="T">Объект данных, отображаемый в гриде</typeparam>
    public interface ISenderView<T> where T : class, IDbEntity, IProfileSendable, new()
    {
        /// <summary>
        /// Очистить грид
        /// </summary>
        void ClearGrid();

        /// <summary>
        /// Добавить объекты в грид
        /// </summary>
        /// <param name="itemsList"></param>
        void AddItemsToGrid(IList<T> itemsList);

        /// <summary>
        /// Обновить состояние грида
        /// </summary>
        void UpdateGridModel();

        /// <summary>
        /// Событие загрузки данных из ВК
        /// </summary>
        event EventHandler<StartLoadBaseEventArgs> StartLoad;

        /// <summary>
        /// Событие инициирования отправки сообщений
        /// </summary>
        event EventHandler<SendMessagesEventArgs<T>> Send;

        /// <summary>
        /// Событие сохранения текущего состояния данных в БД
        /// </summary>
        event EventHandler<SaveBaseEventArgs> Save;

        /// <summary>
        /// Установить сообщение в представлении
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        void SetMessage(string message);

        /// <summary>
        /// Отключить все возможные действия
        /// </summary>
        void DisableAllActions();

        /// <summary>
        /// Включить все возможные действия
        /// </summary>
        void EnableAllActions();

        /// <summary>
        /// Включить действие отмены
        /// </summary>
        /// <param name="cancelAction">Тип включаемого действия отмены</param>
        void EnableCancelAction(ViewCancelActions cancelAction);

        /// <summary>
        /// Отключить действие отмены
        /// </summary>
        /// <param name="cancelAction">Тип выключаемого действия отмены</param>
        void DisableCancelAction(ViewCancelActions cancelAction);

        /// <summary>
        /// События отмены текущего действия
        /// </summary>
        event EventHandler CancelAction;

        /// <summary>
        /// Отобразить капчу
        /// </summary>
        /// <param name="img">Байты изображения капчи</param>
        /// <param name="showingInterval">Время отображения капчи, после чего действие </param>
        /// <returns>Ключ капчи, который оператор ввел с картинки</returns>
        string ShowCaptcha(byte[] img, int showingInterval);
    }
}
