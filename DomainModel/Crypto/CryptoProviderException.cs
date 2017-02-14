using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.DomainModel.Crypto
{
    /// <summary>
    /// Исключение, выбрасываемое при шифровании / расшифровании сообщений
    /// </summary>
    public class CryptoProviderException : Exception
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public CryptoProviderException() { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="message">Сообщение об исключении</param>
        public CryptoProviderException(string message) : base(message) { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="message">Сообщение об исключении</param>
        /// <param name="innerException">Внутреннее исключение</param>
        public CryptoProviderException(string message, Exception innerException) : base(message, innerException) { }
    }
}
