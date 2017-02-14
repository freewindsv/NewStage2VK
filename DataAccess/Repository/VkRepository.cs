using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

using NewStage2VK.DataAccess.DataModel;
using NewStage2VK.DataAccess.Repository.Abstract;

namespace NewStage2VK.DataAccess.Repository
{
    /// <summary>
    /// Репозиторий доступа к сущностям ВК
    /// </summary>
    /// <typeparam name="T">Тип сущности ВК</typeparam>
    public class VkRepository<T> : Repository<T>, IVkRepository<T> where T : class, IVkEntity
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст доступа к БД</param>
        internal VkRepository(NewStage2DbContext context) : base(context) { }

        /// <summary>
        /// Получить асинхронно сущность ВК по ее идентификатору
        /// </summary>
        /// <param name="vkId">Идентификатор запрашиваемой ВК-сущности</param>
        /// <returns>Задача, результат выполнения которой содержит ВК-сущность</returns>
        public virtual async Task<T> GetByVkIdAsync(int vkId)
        {
            return await db.Set<T>().FirstOrDefaultAsync(x => x.VkId == vkId).ConfigureAwait(false);
        }
    }
}
