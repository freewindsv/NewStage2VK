using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.DomainModel
{
    /// <summary>
    /// Комментарий в ВК
    /// </summary>
    public class VkComment : IVkEntity, IVkProfileReferencable
    {
        /// <summary>
        /// Идентификатор 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Дата комментария
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Текст комментария
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Профиль пользователя, оставившего комментарий
        /// </summary>
        public VkProfileBase Profile { get; set; }
    }
}
