using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewStage2VK.DomainModel
{
    /// <summary>
    /// Класс учетных данных оператора
    /// </summary>
    public class Identity
    {
        /// <summary>
        /// Токен для запросов VK-API
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// ИД пользователя (оператора)
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Время действия токена
        /// </summary>
        public TimeSpan? ExpireIn { get; set; }
    }
}
