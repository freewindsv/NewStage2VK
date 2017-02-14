using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.ViewModel
{
    /// <summary>
    /// Тип отменяемого действия
    /// </summary>
    public enum ViewCancelActions
    {
        /// <summary>
        /// Нет
        /// </summary>
        None,

        /// <summary>
        /// Загрузка
        /// </summary>
        Load,

        /// <summary>
        /// Сохранение
        /// </summary>
        Save,

        /// <summary>
        /// рассылка
        /// </summary>
        Send
    }
}
