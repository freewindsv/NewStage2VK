using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.DataAccess.DataModel
{
    /// <summary>
    /// Класс сообщения для рассылки приглашений
    /// </summary>
    public class Message : IDbEntity
    {
        /// <summary>
        /// Идентификатор сущности в VK
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Текст сообщения
        /// </summary>
        [MaxLength(4000)]
        public string Text { get; set; }

        /// <summary>
        /// Тип присутствия пользователя
        /// </summary>
        [Required]
        public PresenceType PresenceType { get; set; }
    }
}
