using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.DomainModel
{
    /// <summary>
    /// Интерфейс сущности ВК
    /// </summary>
    public interface IVkEntity
    {
        /// <summary>
        /// Идентификатор ВК
        /// </summary>
        int Id { get; }
    }
}
