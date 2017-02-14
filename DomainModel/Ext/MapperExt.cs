using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NewStage2VK.DataAccess.DataModel;
using NewStage2VK.DomainModel.Crypto;

namespace NewStage2VK.DomainModel.Ext
{
    /// <summary>
    /// Класс методов расширений
    /// </summary>
    public static class MapperExt
    {
        /// <summary>
        /// Обновить данные пользователя
        /// </summary>
        /// <param name="vkUser">Пользователь, полученный через ВК-API</param>
        /// <param name="user">Пользователь, данные которого нужно обновить</param>
        /// <param name="encAccessToken">Зашифрованный токен доступа</param>
        /// <param name="expireIn">Время действия токена</param>
        /// <param name="imageBytes">Изображение аватарки</param>
        public static void UpdateUser(this VkUser vkUser, User user, string encAccessToken = null, TimeSpan? expireIn = null, byte[] imageBytes = null)
        {
            user.VkId = vkUser.Id;
            user.FirstName = vkUser.FirstName;
            user.LastName = vkUser.LastName;
            user.EncryptedAccessToken = encAccessToken;
            user.ExpireIn = expireIn.HasValue ? (int)expireIn.Value.TotalSeconds : 0;
            user.Avatar_50 = imageBytes;
            user.ModificationDate = DateTime.Now;
        }

        /// <summary>
        /// Обновить профиль
        /// </summary>
        /// <param name="vkProfile">Профиль пользователя, полученный через ВК-API</param>
        /// <param name="profile">Профиль, данные которого нужно обновить</param>
        /// <param name="updateAllowPrivateMessages">Флаг, показывающий, обновлять ли поле AllowPrivateMessages</param>
        /// <param name="imageBytes">Изображение аватарки</param>
        /// <returns>True, если хотя бы 1 поле обновлялось</returns>
        public static bool UpdateProfile(this VkProfileBase vkProfile, Profile profile, bool updateAllowPrivateMessages, byte[] imageBytes = null)
        {
            VkUser vkUser = vkProfile as VkUser;
            bool modified = false;
            if (vkUser != null)
            {
                if (profile.Type != DataAccess.DataModel.ProfileType.User)
                {
                    profile.Type = DataAccess.DataModel.ProfileType.User;
                    modified = true;
                }
                var sex = (DataAccess.DataModel.Sex)vkUser.Sex;
                if (profile.Sex != sex)
                {
                    profile.Sex = sex;
                    modified = true;
                }
                if (updateAllowPrivateMessages && profile.AllowPrivateMessages != vkUser.AllowPrivateMessages)
                {
                    profile.AllowPrivateMessages = vkUser.AllowPrivateMessages;
                    modified = true;
                }
            }
            else
            {
                if (profile.Type != DataAccess.DataModel.ProfileType.Community)
                {
                    profile.Type = DataAccess.DataModel.ProfileType.Community;
                    modified = true;
                }
                if (profile.Sex != DataAccess.DataModel.Sex.Unknown)
                {
                    profile.Sex = DataAccess.DataModel.Sex.Unknown;
                    modified = true;
                }
                if (profile.AllowPrivateMessages != null)
                {
                    profile.AllowPrivateMessages = null;
                    modified = true;
                }
            }

            string name = vkProfile.GetName();
            if (profile.Name != name)
            {
                profile.Name = name;
                modified = true;
            }
            if (profile.AvatarUrl_50 != vkProfile.AvatarUrl_50)
            {
                profile.AvatarUrl_50 = vkProfile.AvatarUrl_50;
                modified = true;
            }
            if (imageBytes != null && profile.Avatar_50 != imageBytes)
            {
                profile.Avatar_50 = imageBytes;
                modified = true;
            }
            return modified;
        }

        /// <summary>
        /// Обновить подписку пользователя на спектакль (или отписку)
        /// </summary>
        /// <param name="vkComment">Комментарий к посту события, полученный через ВК-API</param>
        /// <param name="evtMsg">Объект подписки на спектакль</param>
        /// <returns>True, если хотя бы 1 поле обновлялось</returns>
        public static bool UpdateEventMessage(this VkComment vkComment, EventMessage evtMsg)
        {
            bool modified = false;
            if (!vkComment.Text.Equals(evtMsg.Comment, StringComparison.Ordinal))
            {
                evtMsg.Comment = vkComment.Text;
                modified = true;
            }
            if (vkComment.DateTime != evtMsg.CommentDate)
            {
                evtMsg.CommentDate = vkComment.DateTime;
                modified = true;
            }
            return modified;
        }

        /// <summary>
        /// Получить учетные данные для оператора
        /// </summary>
        /// <param name="user">Объект пользователя (оператора)</param>
        /// <param name="crypto">Объект шифровки / дешифровки токена</param>
        /// <returns>Объект учетных данных</returns>
        public static Identity GetIdentity(this User user, ICryptoProvider crypto)
        {
            return new Identity()
            {
                UserId = user.VkId,
                ExpireIn = user.ExpireIn > 0 ? (TimeSpan?)TimeSpan.FromSeconds(user.ExpireIn) : null,
                AccessToken = crypto.Derypt(user.EncryptedAccessToken)
            };
        }
    }
}
