using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NewStage2VK.DataAccess.DataModel;

namespace NewStage2VK.ViewModel
{
    /// <summary>
    /// Аргументы события сохранения состояния рассылки уведомлений среди "ресурсов" театра
    /// </summary>
    public class SaveInviteEventArgs : SaveBaseEventArgs
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="updateAvatars"> Флаг, указывающий на необходимость обновления аватарок</param>
        /// <param name="type">Тип присутствия пользователя</param>
        public SaveInviteEventArgs(string message, bool updateAvatars, PresenceType type)
            : base(message, updateAvatars)
        {
            this.PresenceType = type;
        }

        /// <summary>
        /// Тип присутствия пользователя
        /// </summary>
        public PresenceType PresenceType { get; private set; }
    }
}
