using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.DataAccess.DataModel
{
    /// <summary>
    /// Класс, который определяет состояние рассылки для пользователей в группе и / или в друзьях. 
    /// </summary>
    public class ProfileMessage : IDbEntity, IProfileSendable
    {
        /// <summary>
        /// Идентификатор, ключ в БД
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Дата и время последней отправки сообщения
        /// </summary>
        public DateTime? SendMessageDate { get; set; }

        /// <summary>
        /// Признак необходимости отправки сообщения
        /// </summary>
        [Required]
        public bool IsSend { get; set; }

        /// <summary>
        /// Признак, было ли отправлено сообщение
        /// </summary>
        [Required]
        public bool WasSent { get; set; }

        /// <summary>
        /// Ссылка на профиль пользователя
        /// </summary>
        [Required]
        public Profile Profile { get; set; }

        /// <summary>
        /// Ссылка на сообщение
        /// </summary>
        [Required]
        public Message Message { get; set; }
    }
}
