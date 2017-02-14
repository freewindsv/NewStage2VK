using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.DomainModel
{
    /// <summary>
    /// Профиль-группа
    /// </summary>
    public class VkGroup : VkProfileBase
    {
        /// <summary>
        /// Имя группы
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Вернуть имя профиля
        /// </summary>
        /// <returns>Название профиля</returns>
        public override string GetName() 
        {
            return Name;
        }
    }
}
