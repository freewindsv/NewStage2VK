using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using NewStage2VK.DataAccess;
using NewStage2VK.DomainModel.Crypto;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace NewStage2VK.DomainModel
{
    /// <summary>
    /// Доменная модель приложения
    /// </summary>
    public class VkDomainModel : IVkDomainModel
    {
        private class ResourceWebClient : WebClient
        {
            private int timeout;

            public ResourceWebClient(int timeout)
            {
                this.timeout = timeout;
            }

            protected override WebRequest GetWebRequest(Uri uri)
            {
                WebRequest w = base.GetWebRequest(uri);
                w.Timeout = timeout;
                return w;
            }
        }

        private ModelConfig config;
        private Identity identity;
        private DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        private DateTime lastRequest;
        private TimeSpan? tsDelay;
        private ResourceWebClient rscWebClient;
        private string apiVersion;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="cfg">Объект настроек модели</param>
        public VkDomainModel(ModelConfig cfg)
        {
            config = cfg;
            lastRequest = origin;
            tsDelay = cfg.RequestDelay > 0 ? (TimeSpan?)TimeSpan.FromMilliseconds(cfg.RequestDelay) : null;
            rscWebClient = new ResourceWebClient(config.RequestResourceTimeout);
            
            Regex reApiVersion = new Regex(@"v=(\d+.\d+)");
            Match m = reApiVersion.Match(cfg.LoginUrl);
            if (m.Success)
            {
                apiVersion = m.Groups[1].Value;
            }
        }

        /// <summary>
        /// Интерфейс доступа к сохраненным данным
        /// </summary>
        public IDataAccess DataAccess
        {
            get { return config.DataAccess; }
        }

        /// <summary>
        /// Интерфейс доступа к методам шифрования / расшифрования текста
        /// </summary>
        public ICryptoProvider CryptoProvider
        {
            get { return config.CryptoProvider; }
        }

        /// <summary>
        /// Url для перенаправления неавторизованного пользователя
        /// </summary>
        public string LoginUrl
        {
            get { return config.LoginUrl; }
        }

        /// <summary>
        /// Учетные данные залогиненного оператора
        /// </summary>
        public Identity LoggedUser
        {
            get { return identity; }
            set { identity = value; }
        }

        /// <summary>
        /// Асинхронная загрузка изображения
        /// </summary>
        /// <param name="url">Url изображения</param>
        /// <returns>Задача, результат которой содержит байты загруженного изображения</returns>
        public async Task<byte[]> GetImageAsync(string url)
        {
            try
            {
                return await rscWebClient.DownloadDataTaskAsync(url);
            }
            catch(Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Получить информацию о пользователе
        /// </summary>
        /// <param name="id">Идентификатор пользователя в сети ВК</param>
        /// <returns>Задача, результат выполнения которой содержит объект результата запроса к ВК-API, который в свою очередь инкапсулирует данные о пользователе</returns>
        public async Task<VkResult<VkUser>> GetUserInfoAsync(int id)
        {
            VkResult<VkUser> results = new VkResult<VkUser>(); 
            string url = $"method/users.get?user_id={identity.UserId}&fields=photo_50,can_write_private_message,sex";
            dynamic obj = await MakeWebRequestAsync(url).ConfigureAwait(false);
            if (obj.error != null)
            {
                results.Error = new VkError()
                {
                    Code = obj.error.error_code,
                    Message = obj.error.error_msg
                };
            }
            if (obj.response != null)
            {
                results.Value = GetVkUser(obj.response[0]);
            }
            return results;
        }

        /// <summary>
        /// Загрузить асинхронно коментарии к посту
        /// </summary>
        /// <param name="ownerId">Идентификатор события в ВК</param>
        /// <param name="postId">Идентификатор поста события в ВК</param>
        /// <param name="type">Тип профиля</param>
        /// <param name="offset">Смещение данных при запросе (например, когда используется разбивка на страницы)</param>
        /// <param name="count">Количество комментариев, которые необходимо вернуть</param>
        /// <returns>Задача, результат выполнения которой содержит объект результата запроса к ВК-API, который в свою очередь инкапсулирует данные о комментариях</returns>
        public async Task<VkResult<VkCollection<VkComment>>> GetCommentsAsync(int ownerId, int postId, ProfileType type, int offset = 0, int count = 100)
        {
            VkResult<VkCollection<VkComment>> results = new VkResult<VkCollection<VkComment>>();           
            if (type == ProfileType.Community)
            {
                ownerId = -ownerId;
            }
            string url = $"method/wall.getComments?owner_id={ownerId}&post_id={postId}&count={count}&offset={offset}&extended=1&sort=asc";
            dynamic obj = await MakeWebRequestAsync(url).ConfigureAwait(false);
            if (obj.error != null)
            {
                results.Error = new VkError()
                {
                    Code = obj.error.error_code,
                    Message = obj.error.error_msg
                };
            }
            else if (obj.response != null)
            {
                results.Value = new VkCollection<VkComment>()
                {
                    TotalCount = obj.response.count,
                    Items = new List<VkComment>()
                };

                dynamic profiles = obj.response.profiles;
                int profileId;
                Dictionary<int, VkUser> dictUsers = new Dictionary<int, VkUser>();
                foreach(var profile in profiles)
                {
                    profileId = profile.id;
                    if (!dictUsers.ContainsKey(profileId))
                    {
                        dictUsers.Add(profileId, GetVkUser(profile));
                    }
                }
                dynamic groups = obj.response.groups;
                Dictionary<int, VkGroup> dictGroups = new Dictionary<int, VkGroup>();
                foreach(var group in groups)
                {
                    profileId = group.id;
                    if (!dictGroups.ContainsKey(profileId))
                    {
                        dictGroups.Add(profileId, new VkGroup()
                        { 
                            Id = profileId, 
                            Name = group.name,
                            AvatarUrl_50 = group.photo_50
                        });
                    }
                }

                dynamic items = obj.response.items;
                int fromId;
                foreach(var item in items)
                {
                    fromId = item.from_id;
                    results.Value.Items.Add(new VkComment()
                    {
                        Id = item.id,
                        Text = item.text,
                        DateTime = ConvertFromUnixTimestamp((double)item.date),
                        Profile = fromId > 0 ? (VkProfileBase)dictUsers[fromId] : (VkProfileBase)dictGroups[-fromId]
                    });
                }
            }
            return results;
        }

        /// <summary>
        /// Отправить асинхронно сообщение пользователю
        /// </summary>
        /// <param name="userId">Идентификатор пользователя в ВК</param>
        /// <param name="message">Текст сообщения</param>
        /// <param name="captchaSid">Идентификатор капчи, если предыдущий запрос вернул ошибку, сообщающую о необходимости ввода капчи</param>
        /// <param name="captchaKey">Ключ капчи с картинки</param>
        /// <returns>Задача, результат выполнения которой содержит объект ошибки при отправки сообщения</returns>
        public async Task<VkError> SendMessageAsync(int userId, string message, string captchaSid = null, string captchaKey = null)
        {
            if (config.MessageDebugVkUserId != 0)
            {
                userId = config.MessageDebugVkUserId;
            }

            VkError error = null;
            string msg = System.Uri.EscapeDataString(message);
            string url = $"method/messages.send?user_id={userId}&message={msg}";
            if (!string.IsNullOrEmpty(captchaSid) && !string.IsNullOrEmpty(captchaKey))
            {
                url += $"&captcha_sid={captchaSid}&captcha_key={captchaKey}";
            }
            dynamic obj = await MakeWebRequestAsync(url).ConfigureAwait(false);
            if (obj.error != null)
            {
                int errorCode = obj.error.error_code;
                if (errorCode == config.CaptchaErrorCode)
                {
                    error = new VkCaptchaError()
                    {
                        Code = obj.error.error_code,
                        Message = obj.error.error_msg,
                        Sid = obj.error.captcha_sid,
                        ImageUrl = obj.error.captcha_img
                    };
                }
                else
                {
                    error = new VkError()
                    {
                        Code = obj.error.error_code,
                        Message = obj.error.error_msg
                    };
                }
            }
            return error;
        }

        /// <summary>
        /// Получить асинхронно число друзей у пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, число друзей у которого необходимо узнать</param>
        /// <returns>Задача, результат выполнения которой содержит объект результата запроса к ВК-API, который в свою очередь инкапсулирует данные о числе друзей</returns>
        public async Task<VkResult<int>> GetFriendsCountAsync(int userId)
        {
            VkResult<int> results = new VkResult<int>();
            string url = $"method/friends.get?user_id={userId}";
            dynamic obj = await MakeWebRequestAsync(url).ConfigureAwait(false);
            if (obj.error != null)
            {
                results.Error = new VkError()
                {
                    Code = obj.error.error_code,
                    Message = obj.error.error_msg
                };
            }
            else if (obj.response != null)
            {
                results.Value = obj.response.count;
            }
            return results;
        }

        /// <summary>
        /// Загрузить асинхронно информацию о друзьях пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, информацию о друзьях которого необходимо узнать</param>
        /// <param name="offset">Смещение данных при запросе (например, когда используется разбивка на страницы)</param>
        /// <param name="count">Количество друзей, которое необходимо вернуть</param>
        /// <returns>Задача, результат выполнения которой содержит объект результата запроса к ВК-API, который в свою очередь инкапсулирует данные о друзьях пользователя</returns>
        public async Task<VkResult<VkCollection<VkUser>>> GetFriendsAsync(int userId, int offset = 0, int count = 100)
        {
            VkResult<VkCollection<VkUser>> results = new VkResult<VkCollection<VkUser>>();
            string url = $"method/friends.get?user_id={userId}&count={count}&offset={offset}&fields=photo_50,can_write_private_message,sex";
            dynamic obj = await MakeWebRequestAsync(url).ConfigureAwait(false);
            if (obj.error != null)
            {
                results.Error = new VkError()
                {
                    Code = obj.error.error_code,
                    Message = obj.error.error_msg
                };
            }
            else if (obj.response != null)
            {
                results.Value = new VkCollection<VkUser>()
                {
                    TotalCount = obj.response.count,
                    Items = new List<VkUser>()
                };

                dynamic users = obj.response.items;
                foreach(var user in users)
                {
                    VkUser vkUser = GetVkUser(user);
                    results.Value.Items.Add(vkUser);
                }
            }
            return results;
        }

        /// <summary>
        /// Получает асинхронно участников группы
        /// </summary>
        /// <param name="groupId">Идентификатор группы в ВК</param>
        /// <param name="offset">Смещение возвращаемой выборки (например, когда используется разбивка на страницы)</param>
        /// <param name="count">Количество результатов (участников), которое нужно вернуть</param>
        /// <returns>Задача, результат выполнения которой содержит объект результата запроса к ВК-API, который в свою очередь инкапсулирует данные об участниках</returns>
        public async Task<VkResult<VkCollection<VkUser>>> GetMembersAsync(int groupId, int offset = 0, int count = 100)
        {
            VkResult<VkCollection<VkUser>> results = new VkResult<VkCollection<VkUser>>();
            string url = $"method/groups.getMembers?group_id={groupId}&count={count}&offset={offset}&fields=photo_50,can_write_private_message,sex";
            dynamic obj = await MakeWebRequestAsync(url).ConfigureAwait(false);
            if (obj.error != null)
            {
                results.Error = new VkError()
                {
                    Code = obj.error.error_code,
                    Message = obj.error.error_msg
                };
            }
            else if (obj.response != null)
            {
                results.Value = new VkCollection<VkUser>()
                {
                    TotalCount = obj.response.count,
                    Items = new List<VkUser>()
                };

                dynamic users = obj.response.items;
                foreach(var user in users)
                {
                    VkUser vkUser = GetVkUser(user);
                    results.Value.Items.Add(vkUser);
                }
            }
            return results;
        }

        /// <summary>
        /// Выполнить асинхронно веб-запрос
        /// </summary>
        /// <param name="url">Url запроса</param>
        /// <returns>Задачу, результат которой содержит данные ответа сервера</returns>
        private async Task<dynamic> MakeWebRequestAsync(string url)
        {
            if (tsDelay.HasValue)
            {
                TimeSpan ts = DateTime.Now - lastRequest;
                if (ts <= tsDelay.Value)
                {
                    await Task.Delay(tsDelay.Value - ts);
                }
                lastRequest = DateTime.Now;
            }

            dynamic obj = null;
            string requestUrl = $"{config.ApiHost}{url}&v={apiVersion}&access_token={identity.AccessToken}";

            try
            {
                using(HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMilliseconds(config.RequestTimeout);
                    using(var response = await client.GetAsync(new Uri(requestUrl)).ConfigureAwait(false))
                    {
                        string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        obj = JObject.Parse(result);
                    }
                }
                return obj;
            }
            catch(Exception ex)
            {
                throw new VkRequestException("Ошибка при запросе к серверу VK", ex);
            }
        }

        /// <summary>
        /// Вернуть пользователя ВК из нетепизированных данных ответа сервера
        /// </summary>
        /// <param name="obj">Динамический объект с данными пользователя</param>
        /// <returns>Объект пользователя ВК</returns>
        private VkUser GetVkUser(dynamic obj)
        {
            VkUser user = new VkUser();
            user.Id = obj.id;
            user.FirstName = obj.first_name;
            user.LastName = obj.last_name;
            user.AvatarUrl_50 = obj.photo_50;
            user.Sex = (Sex)obj.sex;
            user.AllowPrivateMessages = obj.can_write_private_message != null ? 
                (bool?)(obj.can_write_private_message == 0 ? false : true) : null;
            return user;
        }

        /// <summary>
        /// Получить структуру DateTime по смещению в секундах от 01.01.1970г.
        /// </summary>
        /// <param name="timestamp">Смещение в секундах от 01.01.1970г.</param>
        /// <returns>Обычный DateTime</returns>
        private DateTime ConvertFromUnixTimestamp(double timestamp)
        {            
            return origin.AddSeconds(timestamp).ToLocalTime();
        }

    }
}
