using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.ViewModel
{
    /// <summary>
    /// Базовый класс для аргументов события загрузки данных
    /// </summary>
    public class StartLoadBaseEventArgs : EventArgs
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="updateAvatar">Признак необходимости обновления аватарок</param>
        public StartLoadBaseEventArgs(bool updateAvatar)
        {
            this.UpdateAvatar = updateAvatar; 
        }

        /// <summary>
        /// Признак необходимости обновления аватарок
        /// </summary>
        public bool UpdateAvatar { get; private set; }
    }
}
