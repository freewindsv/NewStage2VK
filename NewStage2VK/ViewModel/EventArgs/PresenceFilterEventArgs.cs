using NewStage2VK.DataAccess.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.ViewModel
{
    /// <summary>
    /// Аргументы события смены фильтра в представлении расслыки приглашений
    /// </summary>
    public class PresenceFilterEventArgs : EventArgs
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type">Выбранный тип присутствия в "ресурсах" театра</param>
        public PresenceFilterEventArgs(PresenceType type)
        {
            this.PresenceType = type;
        }

        /// <summary>
        /// Выбранный тип присутствия в "ресурсах" театра
        /// </summary>
        public PresenceType PresenceType { get; private set; }
    }
}
