using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NewStage2VK.DataAccess.DataModel;

namespace NewStage2VK.ViewModel
{
    /// <summary>
    /// Аргументы события сохранения состояния рассылки напоминаний
    /// </summary>
    public class SaveEventMessagesEventArgs : SaveBaseEventArgs
    {     
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="updateAvatars">Флаг, указывающий на необходимость обновления аватарок</param>
        public SaveEventMessagesEventArgs(string message, bool updateAvatars)
            : base(message, updateAvatars)
        {

        }
    }
}
