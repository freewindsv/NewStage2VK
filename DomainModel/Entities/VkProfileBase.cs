using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.DomainModel
{
    /// <summary>
    /// Базовый класс для профиля
    /// </summary>
    public abstract class VkProfileBase : IVkEntity, IVkProfileReferencable
    {
        /// <summary>
        /// Идентификатор сущности ВК
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Url аватарки профиля
        /// </summary>
        public string AvatarUrl_50 { get; set; }

        /// <summary>
        /// Вернуть имя профиля
        /// </summary>
        /// <returns>Название профиля</returns>
        public abstract string GetName();

        /// <summary>
        /// Ссылка на профиль (возвращаем себя же)
        /// </summary>
        public VkProfileBase Profile { get { return this; } }
    }
}
