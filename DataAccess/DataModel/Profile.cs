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
    /// Тип профиля
    /// </summary>
    public enum ProfileType
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        User,
        /// <summary>
        /// Сообщество
        /// </summary>
        Community
    }

    /// <summary>
    /// Пол
    /// </summary>
    public enum Sex
    {
        /// <summary>
        /// Неизвестно
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Женский
        /// </summary>
        Female,
        /// <summary>
        /// Мужской
        /// </summary>
        Male
    }

    /// <summary>
    /// Профиль пользователя
    /// </summary>
    public class Profile : IDbEntity, IVkEntity
    {
        /// <summary>
        /// Ключ в БД
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор в соц. сети ВК
        /// </summary>
        [Required]
        [Index(IsUnique = true)]
        public int VkId { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// Тип профиля
        /// </summary>
        [Required]
        public ProfileType Type { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        [Required]
        public Sex Sex { get; set; }

        /// <summary>
        /// Аватарка пользователя
        /// </summary>
        [Column(TypeName = "varbinary(MAX)")]
        public byte[] Avatar_50 { get; set; }

        /// <summary>
        /// Возможна ли расслылка личных сообщений
        /// </summary>
        public bool? AllowPrivateMessages { get; set; }

        /// <summary>
        /// Присутсвует ли в друзьях
        /// </summary>
        public bool? IsFriend { get; set; }

        /// <summary>
        /// Является ли членом группы
        /// </summary>
        public bool? IsMember { get; set; }

        /// <summary>
        /// Url автврки 50px
        /// </summary>
        [NotMapped]
        public string AvatarUrl_50 { get; set; }
    }
}
