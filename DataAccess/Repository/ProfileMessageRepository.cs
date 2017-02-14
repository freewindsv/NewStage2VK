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
    /// Репозиторий для доступа к профилям пользователей
    /// </summary>
    public class ProfileMessageRepository : Repository<ProfileMessage>, IProfileMessageRepository
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст доступа к БД</param>
        internal ProfileMessageRepository(NewStage2DbContext context) : base(context) { }

        /// <summary>
        /// Получить асинхронно список всех профилей
        /// </summary>
        /// <returns>Задача, результат которой содержит список профилей</returns>
        public async Task<IList<ProfileMessage>> GetMessagesAsync()
        {
            return await db.ProfileMessage.Include(x => x.Profile).ToListAsync();
        }
    }
}
