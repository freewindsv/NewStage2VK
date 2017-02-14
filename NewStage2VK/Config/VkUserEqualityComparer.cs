using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NewStage2VK.DomainModel;

namespace NewStage2VK.Config
{
    /// <summary>
    /// Компаратор, определяющий равенство объектов VkUser
    /// </summary>
    public class VkUserEqualityComparer : IEqualityComparer<VkUser>
    {
        /// <summary>
        /// Метод определяющий равенство
        /// </summary>
        /// <param name="x">Первый объект пользователя ВК</param>
        /// <param name="y">Второй объект пользователя ВК</param>
        /// <returns>True, если объекты равны</returns>
        public bool Equals(VkUser x, VkUser y)
        {
            return x.Id == y.Id;
        }

        /// <summary>
        /// Получение хэш-кода объекта
        /// </summary>
        /// <param name="user">Пользователь ВК</param>
        /// <returns>Хэш-код</returns>
        public int GetHashCode(VkUser user)
        {
            return user.Id;
        }
    }
}
