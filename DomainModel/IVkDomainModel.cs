using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NewStage2VK.DataAccess;
using NewStage2VK.DomainModel.Crypto;

namespace NewStage2VK.DomainModel
{
    /// <summary>
    /// Интерфейс доступа к доменной модели
    /// </summary>
    public interface IVkDomainModel
    {
        /// <summary>
        /// Интерфейс доступа к сохраненным данным
        /// </summary>
        IDataAccess DataAccess { get; }

        /// <summary>
        /// Интерфейс доступа к методам шифрования / расшифрования текста
        /// </summary>
        ICryptoProvider CryptoProvider { get; }

        /// <summary>
        /// Url для перенаправления неавторизованного пользователя
        /// </summary>
        string LoginUrl { get; }

        /// <summary>
        /// Учетные данные залогиненного оператора
        /// </summary>
        Identity LoggedUser { get;set; }

        /// <summary>
        /// Асинхронная загрузка изображения
        /// </summary>
        /// <param name="url">Url изображения</param>
        /// <returns>Задача, результат которой содержит байты загруженного изображения</returns>
        Task<byte[]> GetImageAsync(string url);

        /// <summary>
        /// Получить информацию о пользователе
        /// </summary>
        /// <param name="id">Идентификатор пользователя в сети ВК</param>
        /// <returns>Задача, результат выполнения которой содержит объект результата запроса к ВК-API, который в свою очередь инкапсулирует данные о пользователе</returns>
        Task<VkResult<VkUser>> GetUserInfoAsync(int id);

        /// <summary>
        /// Загрузить асинхронно коментарии к посту
        /// </summary>
        /// <param name="ownerId">Идентификатор события в ВК</param>
        /// <param name="postId">Идентификатор поста события в ВК</param>
        /// <param name="type">Тип профиля</param>
        /// <param name="offset">Смещение данных при запросе (например, когда используется разбивка на страницы)</param>
        /// <param name="count">Количество комментариев, которые необходимо вернуть</param>
        /// <returns>Задача, результат выполнения которой содержит объект результата запроса к ВК-API, который в свою очередь инкапсулирует данные о комментариях</returns>
        Task<VkResult<VkCollection<VkComment>>> GetCommentsAsync(int ownerId, int postId, ProfileType type, int offset = 0, int count = 100);

        /// <summary>
        /// Отправить асинхронно сообщение пользователю
        /// </summary>
        /// <param name="userId">Идентификатор пользователя в ВК</param>
        /// <param name="message">Текст сообщения</param>
        /// <param name="captchaSid">Идентификатор капчи, если предыдущий запрос вернул ошибку, сообщающую о необходимости ввода капчи</param>
        /// <param name="captchaKey">Ключ капчи с картинки</param>
        /// <returns>Задача, результат выполнения которой содержит объект ошибки при отправки сообщения</returns>
        Task<VkError> SendMessageAsync(int userId, string message, string captchaSid = null, string captchaKey = null);

        /// <summary>
        /// Получить асинхронно число друзей у пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, число друзей у которого необходимо узнать</param>
        /// <returns>Задача, результат выполнения которой содержит объект результата запроса к ВК-API, который в свою очередь инкапсулирует данные о числе друзей</returns>
        Task<VkResult<int>> GetFriendsCountAsync(int userId);

        /// <summary>
        /// Загрузить асинхронно информацию о друзьях пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, информацию о друзьях которого необходимо узнать</param>
        /// <param name="offset">Смещение данных при запросе (например, когда используется разбивка на страницы)</param>
        /// <param name="count">Количество друзей, которое необходимо вернуть</param>
        /// <returns>Задача, результат выполнения которой содержит объект результата запроса к ВК-API, который в свою очередь инкапсулирует данные о друзьях пользователя</returns>
        Task<VkResult<VkCollection<VkUser>>> GetFriendsAsync(int userId, int offset = 0, int count = 100);

        /// <summary>
        /// Получает асинхронно участников группы
        /// </summary>
        /// <param name="groupId">Идентификатор группы в ВК</param>
        /// <param name="offset">Смещение возвращаемой выборки (например, когда используется разбивка на страницы)</param>
        /// <param name="count">Количество результатов (участников), которое нужно вернуть</param>
        /// <returns>Задача, результат выполнения которой содержит объект результата запроса к ВК-API, который в свою очередь инкапсулирует данные об участниках</returns>
        Task<VkResult<VkCollection<VkUser>>> GetMembersAsync(int groupId, int offset = 0, int count = 100);
    }
}
