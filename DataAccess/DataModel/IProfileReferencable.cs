using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.DataAccess.DataModel
{
    /// <summary>
    /// Определяет интерфейс, который позволяет получить ссылку на профиль пользователя
    /// </summary>
    public interface IProfileReferencable
    {
        /// <summary>
        /// Ссылка на профиль
        /// </summary>
        Profile Profile { get; }
    }
}
