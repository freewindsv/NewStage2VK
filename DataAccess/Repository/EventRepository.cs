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
    /// Репозиторий для доступа к событиям (встречам)
    /// </summary>
    public class EventRepository : VkRepository<Event>, IEventRepository
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст доступа к БД</param>
        internal EventRepository(NewStage2DbContext context) : base(context) { }

        /// <summary>
        /// Получить асинхронно объект события (встречи)
        /// </summary>
        /// <param name="ownerId">Идентификатор VK владельца</param>
        /// <param name="postId">Идентификатор VK поста</param>
        /// <returns>Задачу, результат которой содержит событие (встречу)</returns>
        public async Task<Event> GetAsync(int ownerId, int postId)
        {
            return await db.Event.FirstOrDefaultAsync(x => x.OwnerId == ownerId && x.PostId == postId).ConfigureAwait(false);
        }
    }
}
