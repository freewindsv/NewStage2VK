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
    /// Класс, который определяет комментарий зрителя под постом о спектакле (класс Event). 
    /// Как правило, это бронирование мест или снятие брони.
    /// </summary>
    public class EventMessage : IDbEntity, IVkEntity, IProfileSendable
    {
        /// <summary>
        /// Идентификатор, ключ в БД
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// VK-идентификатор записи (комментария)
        /// </summary>
        [Required]
        public int VkId { get; set; }

        /// <summary>
        /// Комментарий зрителя
        /// </summary>
        [Required]
        [MaxLength(1024)]
        public string Comment { get; set; }

        /// <summary>
        /// Дата комментария
        /// </summary>
        [Required]
        public DateTime CommentDate { get; set; }

        /// <summary>
        /// Дата, когда зрителю было отправлено последнее сообщение (напоминание)
        /// </summary>
        public DateTime? SendMessageDate { get; set; }

        /// <summary>
        /// Признак необходимости отправки сообщения (напоминания)
        /// </summary>
        [Required]
        public bool IsSend { get; set; }

        /// <summary>
        /// Признак, указывающий, было ли зрителю отправлено сообщение (напоминание)
        /// </summary>
        [Required]
        public bool WasSent { get; set; }

        /// <summary>
        /// Ссылка на профиль зрителя (пользователя)
        /// </summary>
        [Required]
        public Profile Profile { get; set; }

        /// <summary>
        /// Ссылка на событие (встречу), на которое записан зритель
        /// </summary>
        [Required]
        public Event Event { get; set; }
    }
}
