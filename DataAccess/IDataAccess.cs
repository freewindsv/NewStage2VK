using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NewStage2VK.DataAccess.Repository.Abstract;
using NewStage2VK.DataAccess.DataModel;

namespace NewStage2VK.DataAccess
{
    /// <summary>
    /// Интерфейс доступа к сохраненным данным
    /// </summary>
    public interface IDataAccess
    {
        /// <summary>
        /// Репозиторий по управлению пользователем
        /// </summary>
        IVkRepository<User> UserRepository { get; }

        /// <summary>
        /// Репозиторий по управлению настройками
        /// </summary>
        ISettingsRepository SettingsRepository { get; }

        /// <summary>
        /// Репозиторий по управлению мероприятиями
        /// </summary>
        IEventRepository EventRepository { get; }

        /// <summary>
        /// Репозиторий по управлению записями на мероприятия (спектакли)
        /// </summary>        
        IEventMessageRepository EventMessageRepository { get; }

        /// <summary>
        /// Репозиторий по управлению профилями пользователей ВК
        /// </summary>
        IVkRepository<Profile> ProfileRepository { get; }

        /// <summary>
        /// Репозиторий по управлению рассылаемыми сообщениями среди друзей и подписчиков театра
        /// </summary>
        IProfileMessageRepository ProfileMessageRepository { get; }

        /// <summary>
        /// Репозиторий по управлению рассылаемыми сообщениями
        /// </summary>
        IMessageRepository MessageRepository { get; }
    }
}
