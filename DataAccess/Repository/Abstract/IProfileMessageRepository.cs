using NewStage2VK.DataAccess.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.DataAccess.Repository.Abstract
{
    public interface IProfileMessageRepository : IRepository<ProfileMessage>
    {
        Task<IList<ProfileMessage>> GetMessagesAsync();
    }
}
