using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NewStage2VK.DataAccess.DataModel;

namespace NewStage2VK.DataAccess.Repository.Abstract
{
    public interface IEventRepository : IVkRepository<Event>
    {
        Task<Event> GetAsync(int ownerId, int postId);
    }
}
