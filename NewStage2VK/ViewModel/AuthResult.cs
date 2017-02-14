using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewStage2VK.ViewModel
{
    /// <summary>
    /// Статус авторизации
    /// </summary>
    public enum AuthStatus
    {
        /// <summary>
        /// Успех
        /// </summary>
        Success,

        /// <summary>
        /// Начало
        /// </summary>
        Init,

        /// <summary>
        /// Запрос разрешений
        /// </summary>
        RequestPermissions,

        /// <summary>
        /// Доступ запрещен
        /// </summary>
        AccessDenied,

        /// <summary>
        /// Ошибка логина/пароля
        /// </summary>
        WrongPassword,

        /// <summary>
        /// Другая ошибка
        /// </summary>
        Error
    }

    /// <summary>
    /// Результат выполнения авторизации
    /// </summary>
    public class AuthResult
    {
        /// <summary>
        /// Статус
        /// </summary>
        public AuthStatus Status { get; set; }

        /// <summary>
        /// Токен для передачи в запросах ВК-API
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Идентификатор пользователя ВК
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Время действия токена
        /// </summary>
        public int ExpireIn { get; set; }
    }
}
