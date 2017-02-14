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
    /// Событие (встреча) в контакте
    /// </summary>
    public class Event : IVkEntity, IDbEntity
    {
        /// <summary>
        /// Идентификатор, ключ в БД
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// VK-идентификатор владельца (создателя) встречи
        /// </summary>
        [Required]
        public int OwnerId { get; set; }

        /// <summary>
        /// VK-идентификатор поста
        /// </summary>
        [Required]
        public int PostId { get; set; }

        /// <summary>
        /// Текст сообщения, которое нужно разослать зрителям
        /// </summary>
        [MaxLength(4000)]
        public string Message { get; set; }

        /// <summary>
        /// Идентификатор сущности в VK
        /// </summary>
        [NotMapped]
        public int VkId { get { return OwnerId; } }
    }
}
