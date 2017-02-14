using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NewStage2VK.DataAccess.DataModel;

namespace NewStage2VK.ViewModel
{
    /// <summary>
    /// Аргументы события запуска рассылки сообщений
    /// </summary>
    /// <typeparam name="T">Тип рассылки</typeparam>
    public class SendMessagesEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="items">Список объектов</param>
        /// <param name="message">Сообщение</param>
        /// <param name="autoSave">Признак автоматического сохранения после отправки</param>
        /// <param name="updateAvatars">Флаг, указывающий на необходимость обновления аватарок</param>
        /// <param name="usersCount">Ограничение кол-ва людей, которым нужно отправить сообщение</param>
        public SendMessagesEventArgs(IList<T> items, string message, bool autoSave, bool updateAvatars, int? usersCount)
        {
            this.Items = items;
            this.Message = message;
            this.AutoSave = autoSave;
            this.UpdateAvatars = updateAvatars;
            this.UsersCount = usersCount;
        }

        /// <summary>
        /// Список объектов
        /// </summary>
        public IList<T> Items { get; private set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Признак автоматического сохранения после отправки
        /// </summary>
        public bool AutoSave { get; private set; }

        /// <summary>
        /// Флаг, указывающий на необходимость обновления аватарок
        /// </summary>
        public bool UpdateAvatars { get; private set; }

        /// <summary>
        /// Ограничение кол-ва людей, которым нужно отправить сообщение
        /// </summary>
        public int? UsersCount { get; private set; }
    }
}
