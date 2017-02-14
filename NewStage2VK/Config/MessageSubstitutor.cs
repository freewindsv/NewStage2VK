using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NewStage2VK.DataAccess.DataModel;

namespace NewStage2VK.Config
{
    /// <summary>
    /// Класс, выполняющий замену ключевых слов в сообщении
    /// </summary>
    public class MessageSubstitutor : ISubstitutor
    {
        private IDictionary<string, string> config;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="config">Словарь, содержащий конфигурацию</param>
        public MessageSubstitutor(IDictionary<string, string> config)
        {
            this.config = config;
        }

        /// <summary>
        /// Выполнение замены ключевых слов
        /// </summary>
        /// <param name="text">Текст для замены</param>
        /// <param name="profile">Данные профиля пользователя</param>
        /// <returns>Текст сообщения с выполненной заменой</returns>
        public string Replace(string text, Profile profile)
        {
            StringBuilder sb = new StringBuilder(text);
            if (config.ContainsKey("substUser"))
            {
                sb.Replace(config["substUser"], profile.Name);
            }
            return sb.ToString();
        }
    }
}
