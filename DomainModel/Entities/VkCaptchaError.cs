using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NewStage2VK.DomainModel;

namespace NewStage2VK.DomainModel
{
    /// <summary>
    /// Ошибка, обозначающая необходимость ввода капчи
    /// </summary>
    public class VkCaptchaError : VkError
    {
        /// <summary>
        /// Идентификатор капчи
        /// </summary>
        public string Sid { get; set; }

        /// <summary>
        /// Url картинки капчи
        /// </summary>
        public string ImageUrl { get; set; }
    }
}
