using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewStage2VK.ViewModel
{
    /// <summary>
    /// Аргументы события окончания авторизации
    /// </summary>
    public class LoginCompletedEventArgs
    {
        /// <summary>
        /// Успех авторизации
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="success">Усчпех авторизации</param>
        public LoginCompletedEventArgs(bool success)
        {
            Success = success;
        }
    }
}
