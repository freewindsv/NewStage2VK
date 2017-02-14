using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.DataAccess.DataModel
{
    /// <summary>
    /// Тип присутствия пользователя
    /// </summary>
    public enum PresenceType
    {
        /// <summary>
        /// В друзьях или в группе
        /// </summary>
        FriendOrGroup = 0,
        /// <summary>
        /// В друзьях и в группе
        /// </summary>
        FriendAndGroup,
        /// <summary>
        /// В друзьях
        /// </summary>
        Friend,
        /// <summary>
        /// В группе
        /// </summary>
        Group,
        /// <summary>
        /// Только в друзьях, но не в группе
        /// </summary>
        FriendNoGroup,
        /// <summary>
        /// Только в группе, но не в друзьях
        /// </summary>
        GroupNoFriend
    }
}
