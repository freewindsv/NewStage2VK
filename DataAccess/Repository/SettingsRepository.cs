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
    /// Репозитория для доступа к текущим настройкам приложения
    /// </summary>
    public class SettingsRepository : Repository<Settings>, ISettingsRepository
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст доступа к БД</param>
        internal SettingsRepository(NewStage2DbContext context) : base(context) { }

        /// <summary>
        /// Получить асинхронно текущие настройки
        /// </summary>
        /// <returns>Задача, результат выполнения которой содержит объект настроек</returns>
        public async Task<Settings> GetCurrentSettingsAsync()
        {
            return await db.Settings.Include(x => x.CurrentUser).FirstOrDefaultAsync().ConfigureAwait(false);
        }
    }
}
