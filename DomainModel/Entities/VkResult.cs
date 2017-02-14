using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.DomainModel
{
    /// <summary>
    /// Результат запроса от ВК-API
    /// </summary>
    /// <typeparam name="T">Тип ВК-сущности</typeparam>
    public class VkResult<T>
    {
        /// <summary>
        /// Объект ошибки, если null, то ошибки нет
        /// </summary>
        public VkError Error { get; set; }

        /// <summary>
        /// Результат выполнения запроса
        /// </summary>
        public T Value { get; set; }
    }
}
