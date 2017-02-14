using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK
{
    /// <summary>
    /// Интерфейс логирования (запись ошибок и данных отладки)
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Записать строку в журнал
        /// </summary>
        /// <param name="text">Текст сообщения</param>
        void WriteLine(string text);

        /// <summary>
        /// Записать объект исключения в журнал
        /// </summary>
        /// <param name="ex">Объект исключения</param>
        void WriteException(Exception ex);
    }
}
