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
    /// Репозиторий для доступа к сообщениям для рассылки приглашений
    /// </summary>
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст доступа к БД</param>
        internal MessageRepository(NewStage2DbContext context) : base(context) { }

        /// <summary>
        /// Получить асинхронно объект сообщения
        /// </summary>
        /// <returns>Задачу, результат которой содержит объект сообщения</returns>
        public async Task<Message> GetMessageAsync()
        {
            return await db.Message.FirstOrDefaultAsync().ConfigureAwait(false);
        }
    }
}
