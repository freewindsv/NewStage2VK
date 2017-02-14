using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NewStage2VK.ViewModel;
using NewStage2VK.DataAccess.DataModel;

namespace NewStage2VK.View
{
    /// <summary>
    /// Интерфейс представления по управлению рассылкой напоминаний
    /// </summary>
    public interface IRemindView : IStatusView, ISenderView<EventMessage>
    {
        
    }
}
