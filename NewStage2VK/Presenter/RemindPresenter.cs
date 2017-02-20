using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NewStage2VK.View;
using NewStage2VK.ViewModel;
using NewStage2VK.DomainModel;
using NewStage2VK.DataAccess.DataModel;
using NewStage2VK.DomainModel.Ext;
using NewStage2VK.DataAccess.Repository.Abstract;
using NewStage2VK.Config;

namespace NewStage2VK.Presenter
{
    /// <summary>
    /// Презентер для взаимодействия с представлением по рассылке напоминаний среди зрителей спектакля
    /// </summary>
    public class RemindPresenter : MessagePresenterBase<VkComment, EventMessage>
    {
        private const int COMMENTS_LOAD_COUNT = 50;

        private IRemindView remindView;
        private Event evt;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="remindView">Представление с элементами управления рассылкой напоминаний</param>
        /// <param name="model">Модель</param>
        /// <param name="cfg">Объект конфигурации презентора</param>
        public RemindPresenter(IRemindView remindView, IVkDomainModel model, MessagePresenterConfig cfg)
            : base(remindView, remindView, model, cfg)
        {
            this.remindView = remindView;
        }       

        #region Overrides

        /// <summary>
        /// Получить репозиторий для доступа к состоянию рассылки напоминаний
        /// </summary>
        /// <returns>Объект репозитория</returns>
        protected override IRepository<EventMessage> GetRepository()
        {
 	        return model.DataAccess.EventMessageRepository;
        }

        /// <summary>
        /// Получить идентификатор сущности в ВК
        /// </summary>
        /// <param name="item">Объект напоминания</param>
        /// <returns>Идентификатор сущности в ВК</returns>
        protected override int GetVkId(EventMessage item)
        {
 	        return item.VkId;
        }

        /// <summary>
        /// Создать объект напоминания о спектакле
        /// </summary>
        /// <param name="vkEntity">Комментарий пользователя в ВК</param>
        /// <returns>Объект напоминания</returns>
        protected override EventMessage CreateItem(VkComment vkEntity)
        {
 	        return new EventMessage(){ VkId = vkEntity.Id, Event = evt };
        }

        /// <summary>
        /// Обновить объект напоминания
        /// </summary>
        /// <param name="vkEntity">Комментарий пользователя в ВК</param>
        /// <param name="item">Объект напоминания, который нужно обновить</param>
        protected override void UpdateItem(VkComment vkEntity, EventMessage item)
        {
            vkEntity.UpdateEventMessage(item); 	         
        }

        #endregion

        #region Load

        /// <summary>
        /// Вернуть текст, который отображается в статусной строке при загрузке данных
        /// </summary>
        /// <returns>Текст для отображение в статусе</returns>
        protected override string GetLoadingText()
        {
 	        return "Получение комментариев";
        }

        /// <summary>
        /// Запуск асинхронной загрузки комментариев пользователей к посту (спектаклю)
        /// </summary>
        /// <param name="args">Аргументы события загрузки данных</param>
        /// <returns>Задача, результат которой содержит список объектов напоминаний</returns>
        protected override async Task<IList<EventMessage>> RunLoad(StartLoadBaseEventArgs args)
        {
            RemindStartLoadEventArgs e = args as RemindStartLoadEventArgs;
            dictItems = null;

            if (config.Profiles == null)
            {
                IList<Profile> profiles = await model.DataAccess.ProfileRepository.GetItemsAsync();
                config.Profiles = profiles.ToDictionary(x => x.VkId);
            }

            Task<VkResult<VkCollection<VkComment>>> taskVkResults = model.GetCommentsAsync(e.OwnerId, e.PostId, DomainModel.ProfileType.Community, 0, COMMENTS_LOAD_COUNT);
            evt = await model.DataAccess.EventRepository.GetAsync(e.OwnerId, e.PostId);
            Task<IList<EventMessage>> taskEventMessages = model.DataAccess.EventMessageRepository.GetMessagesAsync(e.OwnerId, e.PostId);
            await Task.WhenAll(taskVkResults, taskEventMessages);

            if (cancelFlag)
            {
                return null;
            }

            if (evt == null)
            {
                evt = new Event(){ OwnerId = e.OwnerId, PostId = e.PostId };
            }
            else
            {
                remindView.SetMessage(evt.Message);
            }

            IList<EventMessage> eventMessages = taskEventMessages.Result;
            dictItems = eventMessages.ToDictionary(x => x.VkId);

            VkResult<VkCollection<VkComment>> vkResults = taskVkResults.Result;
            if (vkResults.Error != null)
            {
                throw new ApplicationException(vkResults.Error.Message);
            }

            IList<IList<VkComment>> allVkComments = new List<IList<VkComment>>(); 
            allVkComments.Add(vkResults.Value.Items);

            int currentCount = vkResults.Value.Items.Count;
            remindView.InitProgress(vkResults.Value.TotalCount);
            remindView.SetProgressValue(currentCount);

            while (currentCount < vkResults.Value.TotalCount)
            {
                vkResults = await model.GetCommentsAsync(e.OwnerId, e.PostId, DomainModel.ProfileType.Community, currentCount, COMMENTS_LOAD_COUNT);
                if (cancelFlag)
                {
                    return null;
                }
                if (vkResults.Error != null)
                {
                    throw new ApplicationException(vkResults.Error.Message);
                }
                currentCount += vkResults.Value.Items.Count;
                allVkComments.Add(vkResults.Value.Items);
                remindView.SetProgressValue(currentCount);
            }

            var vkComments = allVkComments.SelectMany(x => x).ToList();
            RemoveNonExitingItems(vkComments);
            AddNewItems(vkComments, false);
            eventMessages = dictItems.Select(x => x.Value).OrderBy(x => x.CommentDate).ToList();
            return eventMessages;
        }

        /// <summary>
        /// Вернуть текст, который отображается в статусной строке после окончания загрузки данных
        /// </summary>
        /// <param name="list">Спсиок объектов напоминаний</param>
        /// <returns>Текст статусной строки</returns>
        protected override string GetLoadDoneText(IList<EventMessage> list)
        {
 	        return $"Готово. Отображено {list.Count} комментариев";
        }

        #endregion

        #region Save

        /// <summary>
        /// Метод обработки перед началом сохранения даных
        /// </summary>
        /// <param name="e">Аргументы события сохранения состояния</param>
        /// <returns>False - отменить дальнейшее сохранение</returns>
        protected override bool PreInitSave(SaveBaseEventArgs e)
        {
 	        if (evt == null)
            {
                return false;
            }
            evt.Message = e.Message;
            return true;
        }

        /// <summary>
        /// Асинхронный метод выполнения сохранения данных
        /// </summary>
        /// <param name="eventMessages">Список объектов напоминаний</param>
        /// <param name="e">Аргументы события сохранения состояния</param>
        /// <returns>Задача, которая содержит результат выполнения. True - если сохранение выполнено полностью, False - если была выполнена отмена</returns>
        protected override async Task<bool> RunSave(IList<EventMessage> eventMessages, SaveBaseEventArgs e)
        {
            if (eventMessages != null)
            {
                return await Save(model.DataAccess.EventMessageRepository, eventMessages, e.UpdateAvatars);
            }
            return true;
        }

        #endregion
    }
}
