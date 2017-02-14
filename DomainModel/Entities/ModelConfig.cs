using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NewStage2VK.DataAccess;
using NewStage2VK.DomainModel.Crypto;

namespace NewStage2VK.DomainModel
{
    /// <summary>
    /// Класс настроек для модели
    /// </summary>
    public class ModelConfig
    {
        /// <summary>
        /// Задержка между запросами к серверу ВК, в миллисекундах. Согласно документации VK-API, 
        /// допустимо не более 3-х запросов в секунду к серверу, поэтому уменьшение этого значения может привести к ошибкам при запросах
        /// </summary>
        public int RequestDelay { get; set; }

        /// <summary>
        /// Таймаут запроса, в миллисекундах. Время, за которое должен прийти ответ от сервера. 
        /// Если за указанное время ответ не приходит, то генерируется исключение и прекращается выполняться текущее действие, 
        /// например, прекращается процесс рассылки. Т.е. программа считает, что нет доступа к сети или же к серверу ВК
        /// </summary>
        public int RequestTimeout { get; set; }

        /// <summary>
        /// Таймаут запроса на загрузку ресурсов (в нашем случае - аватарок), в миллисекундах. 
        /// Время, за которое сервер должен вернуть данные. 
        /// </summary>
        public int RequestResourceTimeout { get; set; }

        /// <summary>
        /// Значение кода ошибки VK-API, которое сообщает программе, что необходимо ввести код капчи.
        /// </summary>
        public int CaptchaErrorCode { get; set; }

        /// <summary>
        /// Адрес, на который перенаправляется оператор, если необходимо выполнить авторизацию в ВК.
        /// </summary>
        public string LoginUrl { get; set; }

        /// <summary>
        /// Ссылка на интерфейс доступа к данным
        /// </summary>
        public IDataAccess DataAccess { get; set; }

        /// <summary>
        /// Идентификатор пользователя в ВК для "отладки" рассылки. Если указан "0", то рассылка сообщений будет происходить должным образом, 
        /// т.е. те пользователи, которые отмечены для рассылки и получат сообщения. Если же значение содержит идентификатор пользователя ВК, 
        /// то все сообщения будут отправляться ему.
        /// </summary>
        public int MessageDebugVkUserId { get; set; }

        /// <summary>
        /// Протокол и доменное имя (или IP-адрес) сервера, который обслуживает запросы VK-API
        /// </summary>
        public string ApiHost { get; set; }

        /// <summary>
        /// Ссылка на интерфейс, который используется для шифрования токена
        /// </summary>
        public ICryptoProvider CryptoProvider { get; set; }
    }
}
