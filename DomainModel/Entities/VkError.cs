using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.DomainModel
{
    /// <summary>
    /// Ошибка ответа от ВК-API
    /// </summary>
    public class VkError
    {
        /// <summary>
        /// Код ошибки
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Текст сообщения об ошибке
        /// </summary>
        public string Message { get; set; }
    }
}
