using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.DataAccess.DataModel
{
    /// <summary>
    /// Определяет интерфейс, содержащий поля с информацией о состоянии рассылки
    /// </summary>
    public interface IProfileSendable
    {
        /// <summary>
        /// Ссылка на профиль пользователя
        /// </summary>
        Profile Profile { get; set; }

        /// <summary>
        /// Признак необходимости отправки сообщения
        /// </summary>
        bool IsSend { get; set; }

        /// <summary>
        ///  Признак, указывающий, было ли зрителю отправлено сообщение (напоминание)
        /// </summary>
        bool WasSent { get; set; }

        /// <summary>
        /// Дата, когда зрителю было отправлено последнее сообщение (напоминание)
        /// </summary>
        DateTime? SendMessageDate { get; set; }
    }
}
