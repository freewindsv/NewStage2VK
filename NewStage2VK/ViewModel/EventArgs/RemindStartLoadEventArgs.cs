using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.ViewModel
{
    /// <summary>
    /// Аргументы события начала загрузки комментариев к посту события (запись на спектакли)
    /// </summary>
    public class RemindStartLoadEventArgs : StartLoadBaseEventArgs
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ownerId">Идентификатор события в ВК</param>
        /// <param name="postId">Идентификатор поста в ВК</param>
        /// <param name="updateAvatar">Признак необходимости обновления аватарок</param>
        public RemindStartLoadEventArgs(int ownerId, int postId, bool updateAvatar)
            : base(updateAvatar)
        {
            this.OwnerId = ownerId;
            this.PostId = postId;            
        }

        /// <summary>
        /// Идентификатор события в ВК
        /// </summary>
        public int OwnerId { get; private set; }

        /// <summary>
        /// Идентификатор поста в ВК
        /// </summary>
        public int PostId { get; private set; }
    }
}
