using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.DataAccess.DataModel
{
    /// <summary>
    /// Определяет интерфейс сущности в БД
    /// </summary>
    public interface IDbEntity
    {
        /// <summary>
        /// Идентификатор (ключ) сущности в БД
        /// </summary>
        int Id { get; set; }
    }
}
