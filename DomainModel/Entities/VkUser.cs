using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewStage2VK.DomainModel
{
    /// <summary>
    /// Профиль пользователя
    /// </summary>
    public class VkUser : VkProfileBase
    {
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        public Sex Sex { get; set; }

        /// <summary>
        /// Возможна ли отправка личных сообщений
        /// </summary>
        public bool? AllowPrivateMessages { get; set; }

        /// <summary>
        /// Вернуть имя профиля
        /// </summary>
        /// <returns>Возвращает имя и фамилию пользователя</returns>
        public override string GetName()
        {
            return FirstName + " " + LastName;
        }
    }
}
