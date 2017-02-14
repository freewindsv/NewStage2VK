using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.DomainModel
{
    /// <summary>
    /// Ссылка на профиль
    /// </summary>
    public interface IVkProfileReferencable
    {
        /// <summary>
        /// Базовый класс профиля
        /// </summary>
        VkProfileBase Profile { get; }
    }
}
