using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.DataAccess.Repository.Abstract
{
    public interface IRepository<T> : IDisposable where T : class
    {
        Task<IList<T>> GetItemsAsync();
        Task<T> GetByIdAsync(int id);
        void Create(T item);
        void Update(T item);
        Task DeleteAsync(int id);
        Task SaveAsync();
    }
}
