using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.DataAccess.DataModel
{
    /// <summary>
    /// Интерфейс, определяющий ключевое поле в VK
    /// </summary>
    public interface IVkEntity
    {
        /// <summary>
        /// Идентификатор сущности в VK
        /// </summary>
        int VkId { get; }
    }
}
