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
    /// Репозиторий для доступа к записям зритилей (коментариям) на спектакль
    /// </summary>
    public class EventMessageRepository : VkRepository<EventMessage>, IEventMessageRepository
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст доступа к БД</param>
        internal EventMessageRepository(NewStage2DbContext context) : base(context) { }

        /// <summary>
        /// Получить асинхронно список записей зрителей (коментариев)
        /// </summary>
        /// <param name="ownerId">Идентификатор VK владельца</param>
        /// <param name="postId">Идентификатор VK поста</param>
        /// <returns>Задача, результат которой содержит список зрителей (коментариев)</returns>
        public async Task<IList<EventMessage>> GetMessagesAsync(int ownerId, int postId)
        {
            return await db.EventMessage.Where(x => x.Event.OwnerId == ownerId && x.Event.PostId == postId).
                Include(x => x.Event).Include(x => x.Profile).ToListAsync();
        }
    }
}
