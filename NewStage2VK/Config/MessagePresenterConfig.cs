using NewStage2VK.DataAccess.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.Config
{
    /// <summary>
    /// Конфигурация презенторов
    /// </summary>
    public class MessagePresenterConfig
    {
        /// <summary>
        /// Задержка между отправкой сообщений
        /// </summary>
        public int SendMessageDelay { get; set; }

        /// <summary>
        /// Время показа капчи
        /// </summary>
        public int CaptchaDelay { get; set; }

        /// <summary>
        /// Объект, выполняющий замену ключевых слов в сообщении
        /// </summary>
        public ISubstitutor Substitutor { get; set; }

        /// <summary>
        /// Объект для ведения протокола работы программы
        /// </summary>
        public ILogger Log { get; set; }

        /// <summary>
        /// Профили пользователей
        /// </summary>
        public IDictionary<int, Profile> Profiles { get; set; }

        /// <summary>
        /// Идентификатор пользователя театра в ВК
        /// </summary>
        public int VkUserId { get; set; }

        /// <summary>
        /// Идентификатор группы театра в ВК
        /// </summary>
        public int VkGroupId { get; set; }
    }
}
