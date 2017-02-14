using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.View
{
    /// <summary>
    /// Интерфейс представления, содержащего информационную статусную строку, а также имеет возможность отображать сообщения
    /// </summary>
    public interface IStatusView : IView
    {
        /// <summary>
        /// Отобразить сообщение как ошибку
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        void ShowError(string message);

        /// <summary>
        /// Отобразить сообщение как предупреждение
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        void ShowWarning(string message);

        /// <summary>
        /// Установить текст статуса
        /// </summary>
        /// <param name="text">Текст статуса</param>
        void SetStatusText(string text);

        /// <summary>
        /// Инициализировать индикатор прогресса
        /// </summary>
        /// <param name="maxValue">Максимальное значение, соответсвующее 100%</param>
        void InitProgress(int maxValue);

        /// <summary>
        /// Установить текущее значение индикатора прогресса
        /// </summary>
        /// <param name="progress">Текущее значение индикатора прогресса</param>
        void SetProgressValue(int progress);

        /// <summary>
        /// Сбросить состояние индикатора прогресса
        /// </summary>
        void ResetProgress();
    }
}
