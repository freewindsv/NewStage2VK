using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NewStage2VK.DataAccess.Repository.Abstract;
using NewStage2VK.DataAccess.DataModel;
using NewStage2VK.DataAccess.Repository;
using System.Reflection;
using System.IO;

namespace NewStage2VK.DataAccess
{
    public class DataAccess : IDataAccess
    {
        private NewStage2DbContext context;
        private IVkRepository<User> userRepository;
        private ISettingsRepository settingsRepository;
        private IEventRepository eventRepository;
        private IEventMessageRepository eventMessageRepository;
        private IVkRepository<Profile> profileRepository;
        private IProfileMessageRepository profileMessageRepository;
        private IMessageRepository messageRepository;

        /// <summary>
        /// Конструктор
        /// </summary>
        public DataAccess()
        {
            context = new NewStage2DbContext();

            #if DEBUG
            context.Database.Log = Console.Write;
            #endif
        }

        /// <summary>
        /// Репозиторий по управлению пользователем
        /// </summary>
        public IVkRepository<User> UserRepository
        {
            get 
            {
                if (userRepository == null)
                {
                    userRepository = new VkRepository<User>(context);
                }
                return userRepository;
            }
        }

        /// <summary>
        /// Репозиторий по управлению настройками
        /// </summary>
        public ISettingsRepository SettingsRepository
        {
            get
            {
                if (settingsRepository == null)
                {
                    settingsRepository = new SettingsRepository(context);
                }
                return settingsRepository;
            }
        }


        /// <summary>
        /// Репозиторий по управлению мероприятиями
        /// </summary>
        public IEventRepository EventRepository
        {
            get
            {
                if (eventRepository == null)
                {
                    eventRepository = new EventRepository(context);
                }
                return eventRepository;
            }
        }

        /// <summary>
        /// Репозиторий по управлению записями на мероприятия (спектакли)
        /// </summary>
        public IEventMessageRepository EventMessageRepository
        {
            get
            {
                if (eventMessageRepository == null)
                {
                    eventMessageRepository = new EventMessageRepository(context);
                }
                return eventMessageRepository;
            }
        }

        /// <summary>
        /// Репозиторий по управлению профилями пользователей ВК
        /// </summary>
        public IVkRepository<Profile> ProfileRepository
        {
            get
            {
                if (profileRepository == null)
                {
                    profileRepository = new VkRepository<Profile>(context);
                }
                return profileRepository;
            }
        }

        /// <summary>
        /// Репозиторий по управлению рассылаемыми сообщениями среди друзей и подписчиков театра
        /// </summary>
        public IProfileMessageRepository ProfileMessageRepository
        {
            get
            {
                if (profileMessageRepository == null)
                {
                    profileMessageRepository = new ProfileMessageRepository(context);
                }
                return profileMessageRepository;
            }
        }

        /// <summary>
        /// Репозиторий по управлению рассылаемыми сообщениями
        /// </summary>
        public IMessageRepository MessageRepository
        {
            get
            {
                if (messageRepository == null)
                {
                    messageRepository = new MessageRepository(context);
                }
                return messageRepository;
            }
        }
    }
}
