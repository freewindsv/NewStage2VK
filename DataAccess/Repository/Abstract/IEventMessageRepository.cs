using NewStage2VK.DataAccess.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.DataAccess.Repository.Abstract
{
    public interface IEventMessageRepository : IVkRepository<EventMessage>
    {
        Task<IList<EventMessage>> GetMessagesAsync(int ownerId, int postId);
    }
}
