using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.DomainModel.Crypto
{
    /// <summary>
    /// Интерфейс определяет методы для шифрования / расшифрования текста
    /// </summary>
    public interface ICryptoProvider
    {
        /// <summary>
        /// Зашифровать строку
        /// </summary>
        /// <param name="text">Строка, подлежащая шифрованию</param>
        /// <returns>Зашифрованная строка</returns>
        string Crypt(string text);

        /// <summary>
        /// Расшифровать строку
        /// </summary>
        /// <param name="text">Зашифрованная строка<</param>
        /// <returns>Расшифрованная строка</returns>
        string Derypt(string text);
    }
}
