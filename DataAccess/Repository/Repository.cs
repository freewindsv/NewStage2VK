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
    /// Базовый класс репозитория. 
    /// Содержит основные методы по получению и сохранению данных
    /// </summary>
    /// <typeparam name="T">Тип репозитория</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// Контекст доступа к БД
        /// </summary>
        internal NewStage2DbContext db;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст доступа к БД</param>
        internal Repository(NewStage2DbContext context)
        {
            this.db = context;
        }

        /// <summary>
        /// Получить асинхронно список объектов
        /// </summary>
        /// <returns>Задача, результат выполнения которой содержит список объектов</returns>
        public virtual async Task<IList<T>> GetItemsAsync()
        {
            return await db.Set<T>().ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Получить асинхронно объект по его идентификатору (ключу) в БД
        /// </summary>
        /// <param name="id">Идентификатор объекта в БД</param>
        /// <returns>Задача, результат выполнения которой содержит нужный объект</returns>
        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await db.Set<T>().FindAsync(id).ConfigureAwait(false);
        }

        /// <summary>
        /// Добавить новый объект в набор
        /// </summary>
        /// <param name="item">Добавляемый объект</param>
        public virtual void Create(T item)
        {
            db.Set<T>().Add(item);
        }

        /// <summary>
        /// Обозначить объект как модифицированный
        /// </summary>
        /// <param name="item">Помечаемый объект</param>
        public virtual void Update(T item)
        {
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
        }

        /// <summary>
        /// Асинхронное удаление объекта по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <returns>Непараметеризированная задача</returns>
        public virtual async Task DeleteAsync(int id)
        {
            T item = await GetByIdAsync(id).ConfigureAwait(false);
            if (item != null)
            {
                db.Set<T>().Remove(item);
            }
        }

        /// <summary>
        /// Сохранить все изменения в БД
        /// </summary>
        /// <returns>Непараметеризированная задача</returns>
        public async Task SaveAsync()
        {
            await db.SaveChangesAsync().ConfigureAwait(false);
        }

        private bool disposed = false;

        /// <summary>
        /// Освободить ресурсы (контекст БД)
        /// </summary>
        /// <param name="disposing">Флаг, показывающий, были ли освобождены ресурсы ранее</param>
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        /// <summary>
        /// Освободить ресурсы (контекст БД)
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
