using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.DomainModel
{
    /// <summary>
    /// Исключение, выбрасываемое при выполнении запроса к ВК-API
    /// </summary>
    public class VkRequestException : Exception
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="message">Текст ошибки</param>
        /// <param name="ex">Внутреннее исключение</param>
        public VkRequestException(string message, Exception ex)
            : base(message, ex)
        {
            
        }
    }
}
