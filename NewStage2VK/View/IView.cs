using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.View
{
    /// <summary>
    /// Базовый интерфейс представления
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Запустить метод в потоке пользовательского интерфейса
        /// </summary>
        /// <param name="deleg">Делегат метода для запуска в потоке UI</param>
        /// <param name="args">Параметры метода</param>
        void RunOnUI(Delegate deleg, params object[] args);
    }
}
