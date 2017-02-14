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
    /// Интерфейс представления по управлению рассылкой уведомлений
    /// </summary>
    public interface IInviteView : IStatusView, ISenderView<ProfileMessage>
    {        
        /// <summary>
        /// Событие смены фильтра типа присутствия
        /// </summary>
        event EventHandler<PresenceFilterEventArgs> PresenceFilterChanged;

        /// <summary>
        /// Установить в фильтр заданный тип присутствия
        /// </summary>
        /// <param name="type">Тип присутствия</param>
        void SetFilterPrecenseType(PresenceType type);
    }
}
