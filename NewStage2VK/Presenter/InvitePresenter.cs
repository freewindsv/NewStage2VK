using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NewStage2VK.Config;
using NewStage2VK.DomainModel;
using NewStage2VK.View;
using NewStage2VK.ViewModel;
using NewStage2VK.DataAccess.DataModel;
using NewStage2VK.DomainModel.Ext;
using NewStage2VK.DataAccess.Repository.Abstract;

namespace NewStage2VK.Presenter
{
    /// <summary>
    /// Презентер для взаимодействия с представлением по рассылке уведомлений среди "ресурсов" театра - друзей и участников группы
    /// </summary>
    public class InvitePresenter : MessagePresenterBase<VkUser, ProfileMessage>
    {
        private const int USERS_LOAD_COUNT = 1500;
        private const int MEMBERS_LOAD_COUNT = 1000;

        private IInviteView inviteView;
        private Message msg;
        private VkUserEqualityComparer vkUserEqCmp;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="inviteView">Представление с элементами управлениями рассылкой</param>
        /// <param name="model">Модель</param>
        /// <param name="cfg">Объект конфигурации</param>
        public InvitePresenter(IInviteView inviteView, IVkDomainModel model, MessagePresenterConfig cfg)
            : base(inviteView, inviteView, model, cfg)
        {
            this.vkUserEqCmp = new VkUserEqualityComparer();
            this.inviteView = inviteView;

            this.inviteView.PresenceFilterChanged += inviteView_PresenceFilterChanged;
        }

        #region View handlers

        /// <summary>
        /// Обработчик события смены фильтра типа присутствия
        /// </summary>
        /// <param name="sender">Объект-отправитель события</param>
        /// <param name="e">Аргументы события смены фильтра</param>
        private void inviteView_PresenceFilterChanged(object sender, PresenceFilterEventArgs e)
        {
            if (dictItems != null)
            {
                inviteView.ClearGrid();
                List<ProfileMessage> filterList = GetFilteredList(e.PresenceType);
                inviteView.AddItemsToGrid(filterList);
                inviteView.SetStatusText($"Готово. Отображено {filterList.Count} пользователей");
            }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Получить репозиторий для доступа к состоянию рассылки уведомлений
        /// </summary>
        /// <returns>Объект репозитория</returns>
        protected override IRepository<ProfileMessage> GetRepository()
        {
 	        return model.DataAccess.ProfileMessageRepository;
        }

        /// <summary>
        /// Получить идентификатор сущности в ВК
        /// </summary>
        /// <param name="item">Объект уведомления</param>
        /// <returns>Идентификатор сущности в ВК</returns>
        protected override int GetVkId(ProfileMessage item)
        {
 	        return item.Profile.VkId;
        }

        /// <summary>
        /// Создать объект уведомления
        /// </summary>
        /// <param name="vkEntity">Пользователь в ВК</param>
        /// <returns>Созданный объект уведомления</returns>
        protected override ProfileMessage CreateItem(VkUser vkEntity)
        {
 	        return new ProfileMessage() { Message = msg };
        }

        #endregion

        #region Load

        /// <summary>
        /// Вернуть текст, который отображается в стаусной строке при загрузке данных
        /// </summary>
        /// <returns>Текст для отображение в статусе</returns>
        protected override string GetLoadingText()
        {
 	        return "Загрузка пользователей";
        }

        /// <summary>
        /// Запуск асинхронной загрузки "ресурсов" театра (друзей и подписчиков)
        /// </summary>
        /// <param name="args">Аргументы события загрузки данных</param>
        /// <returns>Задача, результат которой содержит список объектов уведомлений</returns>
        protected override async Task<IList<ProfileMessage>> RunLoad(StartLoadBaseEventArgs args)
        {
            InviteStartLoadEventArgs e = args as InviteStartLoadEventArgs;
            dictItems = null;
            PresenceType currentPresenceType = e.PresenceType;

            if (config.Profiles == null)
            {
                IList<Profile> profiles = await model.DataAccess.ProfileRepository.GetItemsAsync();
                config.Profiles = profiles.ToDictionary(x => x.VkId);
            }

            inviteView.SetStatusText("Получение друзей");

            VkResult<int> totalCountVkResult = await model.GetFriendsCountAsync(config.VkUserId);
            if (cancelFlag)
            {
                return null;
            }
            if (totalCountVkResult.Error != null)
            {
                throw new ApplicationException(totalCountVkResult.Error.Message);
            }
            int totalCount = totalCountVkResult.Value;

            Task<VkResult<VkCollection<VkUser>>> taskVkResults = model.GetFriendsAsync(config.VkUserId, 0, USERS_LOAD_COUNT);
            msg = await model.DataAccess.MessageRepository.GetMessageAsync();
            Task<IList<ProfileMessage>> taskProfileMessages = model.DataAccess.ProfileMessageRepository.GetMessagesAsync();
            await Task.WhenAll(taskVkResults, taskProfileMessages);

            if (cancelFlag)
            {
                return null;
            }

            if (msg == null)
            {
                msg = new Message();
            }
            else
            {
                inviteView.SetMessage(msg.Text);
                inviteView.SetFilterPrecenseType(msg.PresenceType);
                currentPresenceType = msg.PresenceType;
            }

            IList<ProfileMessage> profileMessages = taskProfileMessages.Result;
            dictItems = profileMessages.ToDictionary(x => x.Profile.VkId);

            VkResult<VkCollection<VkUser>> vkResults = taskVkResults.Result;
            if (vkResults.Error != null)
            {
                throw new ApplicationException(vkResults.Error.Message);
            }

            List<IList<VkUser>> allFriendsVkUsers = new List<IList<VkUser>>();
            allFriendsVkUsers.Add(vkResults.Value.Items);

            int currentCount = vkResults.Value.Items.Count;
            inviteView.InitProgress(totalCount);
            inviteView.SetProgressValue(currentCount);

            while (currentCount < totalCount)
            {
                vkResults = await model.GetFriendsAsync(config.VkUserId, currentCount, USERS_LOAD_COUNT);
                if (cancelFlag)
                {
                    return null;
                }
                if (vkResults.Error != null)
                {
                    throw new ApplicationException(vkResults.Error.Message);
                }
                currentCount += vkResults.Value.Items.Count;
                allFriendsVkUsers.Add(vkResults.Value.Items);
                inviteView.SetProgressValue(currentCount);
            }

            inviteView.SetStatusText("Получение подписчиков");
            inviteView.ResetProgress();

            List<IList<VkUser>> allMembersVkUsers = new List<IList<VkUser>>();
            vkResults = await model.GetMembersAsync(config.VkGroupId, 0, MEMBERS_LOAD_COUNT);
            if (cancelFlag)
            {
                return null;
            }
            if (vkResults.Error != null)
            {
                throw new ApplicationException(vkResults.Error.Message);
            }
            allMembersVkUsers.Add(vkResults.Value.Items);

            totalCount = vkResults.Value.TotalCount;
            currentCount = vkResults.Value.Items.Count;
            inviteView.InitProgress(totalCount);
            inviteView.SetProgressValue(currentCount);

            while (currentCount < totalCount)
            {
                vkResults = await model.GetMembersAsync(config.VkGroupId, currentCount, MEMBERS_LOAD_COUNT);
                if (cancelFlag)
                {
                    return null;
                }
                if (vkResults.Error != null)
                {
                    throw new ApplicationException(vkResults.Error.Message);
                }
                currentCount += vkResults.Value.Items.Count;
                allMembersVkUsers.Add(vkResults.Value.Items);
                inviteView.SetProgressValue(currentCount);
            }

            Dictionary<int, VkUser> vkFriends = allFriendsVkUsers.SelectMany(x => x).ToDictionary(x => x.Id);
            Dictionary<int, VkUser> vkMembers = allMembersVkUsers.SelectMany(x => x).ToDictionary(x => x.Id);
            List<VkUser> vkUsers = vkFriends.Select(x => x.Value).Union(vkMembers.Select(x => x.Value), vkUserEqCmp).ToList();
            RemoveNonExitingItems(vkUsers);
            AddNewItems(vkUsers, true);
            foreach(var profMsg in dictItems)
            {
                profMsg.Value.Profile.IsFriend = vkFriends.ContainsKey(profMsg.Key);
                profMsg.Value.Profile.IsMember = vkMembers.ContainsKey(profMsg.Key);
            }

            profileMessages = GetFilteredList(currentPresenceType);
            return profileMessages;
        }

        /// <summary>
        /// Вернуть текст, который отображается в стаусной строке после окончания загрузки данных
        /// </summary>
        /// <param name="list">Спсиок объектов уведомлений</param>
        /// <returns>Текст статусной строки</returns>
        protected override string GetLoadDoneText(IList<ProfileMessage> list)
        {
 	        return $"Готово. Отображено {list.Count} пользователей";
        }

        #endregion

        #region Save

        /// <summary>
        /// Метод обработки перед началом сохранения даных
        /// </summary>
        /// <param name="args">Аргументы события сохранения состояния</param>
        /// <returns>False - отменить дальнейшее сохранение</returns>
        protected override bool PreInitSave(SaveBaseEventArgs args)
        {
            SaveInviteEventArgs e = args as SaveInviteEventArgs;
            if (msg == null)
            {
                msg = new Message();
            }
            msg.Text = e.Message;
            msg.PresenceType = e.PresenceType;
            return true;            
        }

        /// <summary>
        /// Асинхронный метод выполнения сохранения данных
        /// </summary>
        /// <param name="profileMessages">Список объектов уведомлений</param>
        /// <param name="e">Аргументы события сохранения состояния</param>
        /// <returns>Задача, которая содержит результат выполнения. True - если сохранение выполнено полностью, False - если была выполнена отмена</returns>
        protected override async Task<bool> RunSave(IList<ProfileMessage> profileMessages, SaveBaseEventArgs e)
        {
            if (profileMessages != null)
            {
                return await Save(model.DataAccess.ProfileMessageRepository, profileMessages, e.UpdateAvatars);
            }

            model.DataAccess.MessageRepository.Create(msg);
            await model.DataAccess.MessageRepository.SaveAsync();
            return true;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Вернуть список объектов уведомления в зависимости от фильтра по типу присутствия
        /// </summary>
        /// <param name="type">Выбранный тип присутствия пользователя</param>
        /// <returns>Спсиок объектов уведомлений, пользователи которых удовлетворяют условиям фильтра</returns>
        private List<ProfileMessage> GetFilteredList(PresenceType type)
        {
            return dictItems.Select(x => x.Value).Where(x => GetPresencePredicate(x.Profile, type)).ToList();
        }

        /// <summary>
        /// Вернуть предикат в зависимости от текущего фильтра и присутствия пользователя в друзьях или группе
        /// </summary>
        /// <param name="profile">Профиль пользователя</param>
        /// <param name="type">Выбранный тип присутствия пользователя</param>
        /// <returns>True - если профиль удовлетворяет условиям фильтра</returns>
        private bool GetPresencePredicate(Profile profile, PresenceType type)
        {
            switch(type)
            {
                case PresenceType.FriendOrGroup:
                    return profile.IsFriend == true || profile.IsMember == true;
                case PresenceType.FriendAndGroup:
                    return profile.IsFriend == true && profile.IsMember == true;
                case PresenceType.Friend:
                    return profile.IsFriend == true;
                case PresenceType.Group:
                    return profile.IsMember == true;
                case PresenceType.FriendNoGroup:
                    return profile.IsFriend == true && profile.IsMember == false;
                case PresenceType.GroupNoFriend:
                    return profile.IsFriend == false && profile.IsMember == true;
            }
            return false;
        }

        #endregion
    }

}
