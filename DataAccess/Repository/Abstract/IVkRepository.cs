using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NewStage2VK.DataAccess.DataModel;

namespace NewStage2VK.DataAccess.Repository.Abstract
{
    public interface IVkRepository<T> : IRepository<T> where T : class, IVkEntity
    {
        Task<T> GetByVkIdAsync(int vkId);
    }
}
