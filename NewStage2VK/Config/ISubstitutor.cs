using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NewStage2VK.DataAccess.DataModel;

namespace NewStage2VK.Config
{
    /// <summary>
    /// Интерфейс для выполнения подстановок в сообщениях
    /// </summary>
    public interface ISubstitutor
    {
        /// <summary>
        /// Выполнить замену ключевых слов на данные из профиля пользователя
        /// </summary>
        /// <param name="text">Текст с символами подстановки</param>
        /// <param name="profile">Профиль пользователя</param>
        /// <returns>Текст сообщения с выполненной заменой</returns>
        string Replace(string text, Profile profile);
    }
}
