using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.DomainModel
{
    /// <summary>
    /// Коллекция ВК-объектов
    /// </summary>
    /// <typeparam name="T">Тип ВК-объекта</typeparam>
    public class VkCollection<T>
    {
        /// <summary>
        /// Список ВК-объектов
        /// </summary>
        public IList<T> Items { get; set; }

        /// <summary>
        /// Общее кол-во объектов
        /// </summary>
        public int TotalCount { get; set; }
    }
}
