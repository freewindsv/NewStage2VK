using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NewStage2VK.DomainModel;
using NewStage2VK.View;
using NewStage2VK.DataAccess.Repository.Abstract;
using NewStage2VK.DataAccess.DataModel;
using NewStage2VK.Config;
using NewStage2VK.DomainModel.Ext;
using NewStage2VK.ViewModel;

namespace NewStage2VK.Presenter
{
    /// <summary>
    /// Базовый шаблонный класс для презенторов представлений по рассылке сообщений
    /// </summary>
    /// <typeparam name="VK">Тип данных ВК</typeparam>
    /// <typeparam name="T">Тип данных в БД</typeparam>
    public abstract class MessagePresenterBase<VK, T>
        where VK : NewStage2VK.DomainModel.IVkEntity, IVkProfileReferencable
        where T : class, IDbEntity, IProfileSendable, new()
    {
        private const int SAVE_ITEMS_COUNT = 50;
        protected const string CANCEL_STATUS_TEXT = "Отменяем. Подождите немного...";

        private IStatusView statusView;
        private ISenderView<T> senderView;
        private HashSet<int> profileUpdatedIds;
        protected IVkDomainModel model;
        protected MessagePresenterConfig config;
        protected IDictionary<int, T> dictItems;
        protected bool cancelFlag;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="statusView">Интерфейс представления статуса</param>
        /// <param name="senderView">Интерфейс представления области управления рассылкой</param>
        /// <param name="model">Модель</param>
        /// <param name="config">Конфигурация презентора</param>
        public MessagePresenterBase(IStatusView statusView, ISenderView<T> senderView, IVkDomainModel model, MessagePresenterConfig config)
        {
            this.statusView = statusView;
            this.senderView = senderView;
            this.model = model;
            this.config = config;

            this.senderView.StartLoad += senderView_StartLoad;
            this.senderView.Send += senderView_Send;
            this.senderView.Save += senderView_Save;
            this.senderView.CancelAction += senderView_CancelAction;
        }

        #region Abstract / Virtual members

        /// <summary>
        /// Вернуть текст, который отображается в стаусной строке при загрузке данных
        /// </summary>
        /// <returns>Текст для отображение в статусе</returns>
        protected abstract string GetLoadingText();

        /// <summary>
        /// Вернуть текст, который отображается в стаусной строке после окончания загрузки данных
        /// </summary>
        /// <param name="list">Спсиок объектов данных</param>
        /// <returns>Текст статусной строки</returns>
        protected abstract string GetLoadDoneText(IList<T> list);

        /// <summary>
        /// Запуск асинхронной загрузки данных
        /// </summary>
        /// <param name="e">Аргументы события загрузки данных</param>
        /// <returns>Задача, результат которой содержит список объектов данных</returns>
        protected abstract Task<IList<T>> RunLoad(StartLoadBaseEventArgs e);

        /// <summary>
        /// Метод обработки перед началом сохранения даных
        /// </summary>
        /// <param name="e">Аргументы события сохранения состояния</param>
        /// <returns>False - отменить дальнейшее сохранение</returns>
        protected abstract bool PreInitSave(SaveBaseEventArgs e);

        /// <summary>
        /// Асинхронный метод выполнения сохранения данных
        /// </summary>
        /// <param name="items">Список объектов напоминаний</param>
        /// <param name="e">Аргументы события сохранения состояния</param>
        /// <returns>Задача, которая содержит результат выполнения. True - если сохранение выполнено полностью, False - если была выполнена отмена</returns>
        protected abstract Task<bool> RunSave(IList<T> items, SaveBaseEventArgs e);

        /// <summary>
        /// Получить репозиторий для доступа к состоянию рассылки
        /// </summary>
        /// <returns>Объект репозитория</returns>
        protected abstract IRepository<T> GetRepository();

        /// <summary>
        /// Получить идентификатор сущности в ВК
        /// </summary>
        /// <param name="item">Объект данных рассылки</param>
        /// <returns></returns>
        protected abstract int GetVkId(T item);

        /// <summary>
        /// Создать объект данных рассылки
        /// </summary>
        /// <param name="vkEntity">Объект ВК</param>
        /// <returns>Объект данных рассылки</returns>
        protected abstract T CreateItem(VK vkEntity);

        /// <summary>
        /// Обновить объект данных рассылки
        /// </summary>
        /// <param name="vkEntity">Объект ВК</param>
        /// <param name="item">Объект данных рассылки</param>
        protected virtual void UpdateItem(VK vkEntity, T item) { }

        #endregion

        #region Event handlers

        /// <summary>
        /// Обработчик события начала загрузки данных
        /// </summary>
        /// <param name="sender">Объект-отправитель события</param>
        /// <param name="e">Аргументы события загрузки данных</param>
        private async void senderView_StartLoad(object sender, StartLoadBaseEventArgs e)
        {
            InitLoad();
            try
            {
                IList<T> list = await RunLoad(e);
                if (list == null)
                {
                    ActionCanceled(ViewCancelActions.Load);
                }
                else
                {
                    DoneLoad(list);
                }
            }
            catch(Exception ex)
            {
                FailAction(ex, ViewCancelActions.Load);
            }
        }

        /// <summary>
        /// Обработчик события запуска рассылки сообщений
        /// </summary>
        /// <param name="sender">Объект-отправитель события</param>
        /// <param name="e">Аргументы события запуска рассылки сообщений</param>
        private async void senderView_Send(object sender, SendMessagesEventArgs<T> e)
        {
            var profilesSeq = e.Items.
                Where(x => x.IsSend && !x.WasSent && x.Profile.Type == DataAccess.DataModel.ProfileType.User).
                Select(x => x.Profile).Distinct();

            if (e.UsersCount.HasValue && e.UsersCount.Value > 0)
            {
                profilesSeq = profilesSeq.Take(e.UsersCount.Value);
            }

            List<Profile> profiles = profilesSeq.ToList();

            if (profiles.Count == 0)
            {
                statusView.ShowWarning("Нет пользователей для рассылки");
                return;
            }

            InitSend(profiles);
            try
            {
                IList<VkError> errors = await RunSend(profiles, e);
                if (errors == null)
                {
                    ActionCanceled(ViewCancelActions.Send);
                }
                else
                {
                    DoneSend(errors);
                }
            }
            catch(Exception ex)
            {
                FailAction(ex, ViewCancelActions.Send);
            }
        }

        /// <summary>
        /// Обработчик события сохранения состояния рассылки
        /// </summary>
        /// <param name="sender">Объект-отправитель события</param>
        /// <param name="e">Аргументы события сохранения состояния</param>
        private async void senderView_Save(object sender, SaveBaseEventArgs e)
        {
            bool result = PreInitSave(e);
            if (result)
            {
                IList<T> items = dictItems != null ? dictItems.Select(x => x.Value).ToList() : null;
                if (items != null)
                {
 	                InitSave(items);
                    try
                    {
                        bool success = await RunSave(items, e);
                        if (success)
                        {
                            DoneSave();
                        }
                        else
                        {
                            ActionCanceled(ViewCancelActions.Save);
                        }
                    }
                    catch(Exception ex)
                    {
                        FailAction(ex, ViewCancelActions.Save);
                    }
                }
            }
            else
            {
                statusView.SetStatusText("Нечего сохранять");
            }
        }

        /// <summary>
        /// Обработчик события отмены текущего действия
        /// </summary>
        /// <param name="sender">Объект-отправитель события</param>
        /// <param name="e">Пустой объект EventArgs</param>
        private void senderView_CancelAction(object sender, EventArgs e)
        {
            statusView.SetStatusText(CANCEL_STATUS_TEXT);
            cancelFlag = true; 	
        }

        #endregion

        #region Load

        /// <summary>
        /// Инициализация перед стартом загрузки данных из ВК
        /// </summary>
        protected virtual void InitLoad()
        {
            cancelFlag = false;

            senderView.ClearGrid();
            statusView.SetStatusText(GetLoadingText());

            senderView.DisableAllActions();
            senderView.EnableCancelAction(ViewCancelActions.Load);
        }

        /// <summary>
        /// Завершение после загрузки данных из ВК
        /// </summary>
        /// <param name="list">Список объектов данных</param>
        protected virtual void DoneLoad(IList<T> list)
        {
            senderView.AddItemsToGrid(list);

            statusView.SetStatusText(GetLoadDoneText(list));
            statusView.ResetProgress();

            senderView.DisableCancelAction(ViewCancelActions.Load);
            senderView.EnableAllActions();
        }

        #endregion

        #region Send

        /// <summary>
        /// Инициализация перед отправкой сообщений пользователям
        /// </summary>
        /// <param name="profiles">Список профилей пользователей для отправки</param>
        protected virtual void InitSend(IList<Profile> profiles)
        {
            cancelFlag = false;

            statusView.SetStatusText($"Количество отправляемых сообщений: {profiles.Count}");
            statusView.InitProgress(profiles.Count);

            senderView.DisableAllActions();
            senderView.EnableCancelAction(ViewCancelActions.Send);
        }

        /// <summary>
        /// Запуск процесса рассылки сообщений
        /// </summary>
        /// <param name="profiles">Список профилей пользователей для отправки</param>
        /// <param name="e">Аргументы события запуска рассылки сообщений</param>
        /// <returns>Задача, результат которой содержит список ошибок, возникших в процессе рассылки</returns>
        protected virtual async Task<IList<VkError>> RunSend(IList<Profile> profiles, SendMessagesEventArgs<T> e)
        {
            return await SendMessages(profiles, e.Message, async p => {
                var items = dictItems.Where(x => x.Value.Profile.VkId == p.VkId).Select(x => x.Value);
                foreach(T item in items)
                {
                    item.WasSent = true;
                    item.IsSend = false;
                    item.SendMessageDate = DateTime.Now;
                    if (e.AutoSave)
                    {
                        var repository = GetRepository();
                        if (item.Id == 0)
                        {
                            repository.Create(item);
                        }
                        if (e.UpdateAvatars)
                        {
                            byte[] imgBytes = await model.GetImageAsync(item.Profile.AvatarUrl_50);
                        }
                        await repository.SaveAsync();
                    }
                }
                senderView.UpdateGridModel(); 
            });
        }

        /// <summary>
        /// Завершение после рассылки сообщений
        /// </summary>
        /// <param name="errors">Список ошибок во время рассылки</param>
        protected virtual void DoneSend(IList<VkError> errors)
        {
            int errCount = errors.Count;
            string status = errCount > 0 ? $"Готово. Количество ошибок при отправке сообщений: {errCount}" : "Готово";
            statusView.SetStatusText(status);
            statusView.ResetProgress();

            senderView.DisableCancelAction(ViewCancelActions.Send);
            senderView.EnableAllActions();
        }

        #endregion

        #region Save

        /// <summary>
        /// Инициализация перед сохранением данных
        /// </summary>
        /// <param name="items">Спсиок объектов, которые нужно сохранить</param>
        protected virtual void InitSave(IList<T> items)
        {
            cancelFlag = false;

            statusView.SetStatusText("Сохранение в базу данных");
            statusView.InitProgress(items.Count);

            senderView.DisableAllActions();
            senderView.EnableCancelAction(ViewCancelActions.Save);
        }

        /// <summary>
        /// Завершение сохранения
        /// </summary>
        protected virtual void DoneSave()
        {
            statusView.SetStatusText("Готово. Сохранено");
            statusView.ResetProgress();

            senderView.DisableCancelAction(ViewCancelActions.Save);
            senderView.EnableAllActions();
        }

        #endregion

        #region Action fail and cancel

        /// <summary>
        /// Метод вызывается при выбрасывании исключения в процессе выполнения текущего действия
        /// </summary>
        /// <param name="ex">Объект исключения</param>
        /// <param name="cancelAction">Тип отменяемого действия</param>
        protected virtual void FailAction(Exception ex, ViewCancelActions cancelAction)
        {
            string txt = $"Ошибка. {ex.Message}";
            statusView.SetStatusText(txt);
            statusView.ResetProgress();

            if (config.Log != null)
            {
                config.Log.WriteException(ex);
            }

            if (ex is VkRequestException)
            {
                statusView.ShowError("Возникла ошибка со связью. Возможно, нет доступа к Интернет. \r\nПодробности: " + ex.Message);
            }
            else
            {
                statusView.ShowError(txt);
            }

            senderView.DisableCancelAction(cancelAction);
            senderView.EnableAllActions();
        }

        /// <summary>
        /// Метод вызывается при отмене выполнения текущего действия
        /// </summary>
        /// <param name="cancelAction">Тип отменяемого действия</param>
        protected virtual void ActionCanceled(ViewCancelActions cancelAction)
        {
            cancelFlag = false;
            statusView.SetStatusText("Отменено");
            statusView.SetProgressValue(0); 

            senderView.DisableCancelAction(cancelAction);
            senderView.EnableAllActions();
        }

        #endregion

        #region Main logic methods

        /// <summary>
        /// Удалить объекты, котрые не были найдены в текущих объектах ВК
        /// </summary>
        /// <param name="vkItems">Объекты ВК</param>
        protected void RemoveNonExitingItems(IEnumerable<VK> vkItems)
        {
            T item;
            List<int> delIds = dictItems.Select(x => GetVkId(x.Value)).Except(vkItems.Select(x => x.Id)).ToList();
            foreach(int id in delIds)
            {
                item = dictItems[id];
                dictItems.Remove(id);
            }
        }

        /// <summary>
        /// Добавить объекты из объектов ВК с обновлением уже существующих
        /// </summary>
        /// <param name="vkItems">Объекты ВК</param>
        /// <param name="updateAllowPrivateMessages">Флаг, указывающий обновлять ли свойство "AllowPrivateMessages"</param>
        protected void AddNewItems(IEnumerable<VK> vkItems, bool updateAllowPrivateMessages)
        {
            profileUpdatedIds = new HashSet<int>();
            foreach(VK vkItem in vkItems)
            {
                Profile profile;
                if (config.Profiles.ContainsKey(vkItem.Profile.Id))
                {
                    profile = config.Profiles[vkItem.Profile.Id];
                }
                else
                {
                    profile = new Profile()
                    { 
                        VkId = vkItem.Profile.Id
                    };
                    config.Profiles.Add(vkItem.Profile.Id, profile);
                }
                if (!profileUpdatedIds.Contains(vkItem.Profile.Id))
                {
                    vkItem.Profile.UpdateProfile(profile, updateAllowPrivateMessages);
                    profileUpdatedIds.Add(vkItem.Profile.Id);
                }

                T item;
                if (!dictItems.ContainsKey(vkItem.Id))
                {
                    item = CreateItem(vkItem);
                    dictItems.Add(vkItem.Id, item);
                }
                else
                {
                    item = dictItems[vkItem.Id];
                }
                if (item.Profile == null)
                {
                    item.Profile = profile;
                }
                UpdateItem(vkItem, item);
            }
        }

        /// <summary>
        /// Сохранить объекты в БД
        /// </summary>
        /// <param name="repository">Репозиторий, выполняющий операцию сохранения</param>
        /// <param name="itemsList">Спсиок объектов для сохранения</param>
        /// <param name="updateAvatars">Флаг, указывающий обновлять ли аватарки</param>
        /// <returns>Задача, результат которой содержит True, если сохранение прошло успешно. False - если была отмена</returns>
        protected async Task<bool> Save(IRepository<T> repository, IList<T> itemsList, bool updateAvatars)
        {       
            Task<bool> saveTask = Task.Run(async () =>
            {
                int i = 0;
                int count = 0;
                List<Task<byte[]>> tasks = new List<Task<byte[]>>();
 	            foreach(T item in itemsList)
                {
                    count++;
                    if (item.Id == 0)
                    {
                        repository.Create(item);
                    }
                    if (updateAvatars)
                    {
                        Task<byte[]> task = model.GetImageAsync(item.Profile.AvatarUrl_50);
                        tasks.Add(task);
                        Task t2 = task.ContinueWith(t => 
                        { 
                            item.Profile.Avatar_50 = t.Result;
                            RunOnUI(() => 
                            {
                                statusView.SetProgressValue(++i);
                            });
                        });
                    }             
                    if (count % SAVE_ITEMS_COUNT == 0)
                    {
                        if (updateAvatars)
                        {
                            await Task.WhenAll(tasks);
                            tasks.Clear();
                        }
                        else
                        {
                            RunOnUI(() => 
                            {
                                statusView.SetProgressValue(count);
                            });
                            i = count;
                        }
                        await repository.SaveAsync();
                        if (cancelFlag)
                        {
                            return false;
                        }
                    }
                }
                if (count % SAVE_ITEMS_COUNT != 0)
                {
                    await Task.WhenAll(tasks);
                    await repository.SaveAsync();
                    RunOnUI(() => 
                    {
                        statusView.SetProgressValue(count);
                    });
                }
                return true;
            });
            return await saveTask;
        }

        /// <summary>
        /// Отправить сообщения
        /// </summary>
        /// <param name="profiles">Список профилей пользователей</param>
        /// <param name="message">Сообщение</param>
        /// <param name="onSuccess">Ф-ция обратного вызова в случае успешной отправки сообщения</param>
        /// <returns>Задача, результат которой содержит список ошибок при рассылке</returns>
        protected async Task<IList<VkError>> SendMessages(IList<Profile> profiles, string message, Func<Profile, Task> onSuccess)
        {
            TimeSpan? tsMsgDelay = null;
            DateTime lastSendMessage = DateTime.Now.AddYears(-1);
            if (config.SendMessageDelay > 0)
            {
                tsMsgDelay = TimeSpan.FromMilliseconds(config.SendMessageDelay);
            }

            int current = 0;
            List<VkError> errors = new List<VkError>();
            foreach(var profile in profiles)
            {
                if (tsMsgDelay.HasValue)
                {
                    TimeSpan ts = DateTime.Now - lastSendMessage;
                    if (ts <= tsMsgDelay.Value)
                    {
                        await Task.Delay(tsMsgDelay.Value - ts);
                    }
                    lastSendMessage = DateTime.Now;
                }

                string msg = config.Substitutor != null ? config.Substitutor.Replace(message, profile) : message;
                await SendMessageAsync(profile, msg, onSuccess, errors, null, null);
                if (cancelFlag)
                {
                    return null;
                }
                statusView.SetProgressValue(++current);
                statusView.SetStatusText($"Отправлено {current} из {profiles.Count}");
            }
            return errors;
        }

        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="profile">Профиль пользователя</param>
        /// <param name="msg">Сообщение</param>
        /// <param name="onSuccess">Ф-ция обратного вызова в случае успешной отправки сообщения</param>
        /// <param name="errors">Список, в который нужно добавить ошибку, если отправка не удалась</param>
        /// <param name="captchaSid">Идентификатор капчи</param>
        /// <param name="captchaKey">Ключ капчи с изображения</param>
        /// <returns>Задачу</returns>
        private async Task SendMessageAsync(Profile profile, string msg, Func<Profile, Task> onSuccess, IList<VkError> errors, string captchaSid, string captchaKey)
        {
            VkError vkError = await model.SendMessageAsync(profile.VkId, msg, captchaSid, captchaKey);
            if (vkError == null)
            {
                if (onSuccess != null)
                {
                    await onSuccess(profile);
                }
            }
            else
            {
                if (vkError is VkCaptchaError)
                {                        
                    VkCaptchaError captchaError = vkError as VkCaptchaError;
                    byte[] img = await model.GetImageAsync(captchaError.ImageUrl);
                    string key = senderView.ShowCaptcha(img, config.CaptchaDelay);
                    if (string.IsNullOrEmpty(key))
                    {
                        AddError(errors, vkError);
                    }
                    else
                    {
                        await SendMessageAsync(profile, msg, onSuccess, errors, captchaError.Sid, key);
                    }
                }
                else
                {
                    AddError(errors, vkError);
                }
            }
        }

        /// <summary>
        /// Добавить ошибку в список
        /// </summary>
        /// <param name="errors">Список ошибок</param>
        /// <param name="vkError">Ошибка, которую нужно добавить</param>
        private void AddError(IList<VkError> errors, VkError vkError)
        {
            errors.Add(vkError);
            if (config.Log != null)
            {
                config.Log.WriteLine($"Code: {vkError.Code}, Message: {vkError.Message}");
            }
        }

        /// <summary>
        /// Запуск метода в потоке пользовательского интерфейса
        /// </summary>
        /// <param name="action">Делегат метода для запуска в потоке UI</param>
        private void RunOnUI(Action action)
        {
            statusView.RunOnUI(action);
        }

        #endregion
    }
}
