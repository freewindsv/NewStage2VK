using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.ViewModel
{
    /// <summary>
    /// Базовый класс для аргументов события сохранения состояния
    /// </summary>
    public class SaveBaseEventArgs : EventArgs
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="updateAvatars">Флаг, указывающий на необходимость обновления аватарок</param>
        public SaveBaseEventArgs(string message, bool updateAvatars)
        {
            this.UpdateAvatars = updateAvatars;
            this.Message = message;
        }

        /// <summary>
        /// Флаг, указывающий на необходимость обновления аватарок
        /// </summary>
        public bool UpdateAvatars { get; private set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; private set; }
    }
}
