using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace NewStage2VK.DomainModel.Crypto
{
    /// <summary>
    /// Класс для шифрования / расшифрования текста
    /// </summary>
    public class CryptoProvider : ICryptoProvider
    {
        /// <summary>
        /// Зашифровать строку
        /// </summary>
        /// <param name="text">Строка, подлежащая шифрованию</param>
        /// <returns>Зашифрованная строка</returns>
        public string Crypt(string text)
        {
            return Convert.ToBase64String(
                ProtectedData.Protect(Encoding.Unicode.GetBytes(text), null, DataProtectionScope.LocalMachine));
        }

        /// <summary>
        /// Расшифровать строку
        /// </summary>
        /// <param name="text">Зашифрованная строка<</param>
        /// <returns>Расшифрованная строка</returns>
        public string Derypt(string text)
        {
            try
            {
                return Encoding.Unicode.GetString(
                    ProtectedData.Unprotect(Convert.FromBase64String(text), null, DataProtectionScope.LocalMachine));
            }
            catch (CryptographicException ex)
            {
                throw new CryptoProviderException("Failed to unprotect text string", ex);
            }
        }
    }
}
