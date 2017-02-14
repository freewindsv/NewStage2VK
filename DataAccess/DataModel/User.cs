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
    /// Пользователь (оператор) приложения
    /// </summary>
    public class User : IDbEntity, IVkEntity
    {
        /// <summary>
        /// Идентификатор (ключ) в БД
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор пользователя (оператора) в VK
        /// </summary>
        [Required]
        [Index(IsUnique = true)]
        public int VkId { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [MaxLength(256)]
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [MaxLength(256)]
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Байты изображения автарки в 50px
        /// </summary>
        [Column(TypeName = "varbinary(MAX)")]
        public byte[] Avatar_50 { get; set; }

        /// <summary>
        /// Зашифрованный токен для VK-API
        /// </summary>
        [MaxLength(1024)]
        public string EncryptedAccessToken { get; set; }

        /// <summary>
        /// Время жизни токена (в секундах)
        /// </summary>
        [Required]
        public int ExpireIn { get; set; }

        /// <summary>
        /// Дата последнего изменения
        /// </summary>
        [Required]
        public DateTime ModificationDate { get; set; }

        /// <summary>
        /// Полное имя
        /// </summary>
        [NotMapped]
        public string Name { get { return FirstName + " " + LastName; } }
    }
}
