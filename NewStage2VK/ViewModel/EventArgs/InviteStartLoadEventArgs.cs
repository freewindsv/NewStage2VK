using NewStage2VK.DataAccess.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.ViewModel
{
    /// <summary>
    /// Аргументы события начала загрузки друзей и участников группы
    /// </summary>
    public class InviteStartLoadEventArgs : StartLoadBaseEventArgs
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="updateAvatar">Флаг, указывающий на необходимость обновления аватарок</param>
        /// <param name="presenceType">Тип присутствия пользователя в "ресурсах" театра</param>
        public InviteStartLoadEventArgs(bool updateAvatar, PresenceType presenceType)
            : base(updateAvatar)
        {
            this.PresenceType = presenceType;
        }

        /// <summary>
        /// Тип присутствия пользователя в "ресурсах" театра
        /// </summary>
        public PresenceType PresenceType { get; private set; }
    }
}
