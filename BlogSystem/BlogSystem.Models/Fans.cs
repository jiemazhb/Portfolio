using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Models
{
    /// <summary>
    /// 粉丝表
    /// </summary>
    public class Fans : BaseEntity
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User User { get; set; }
        /// <summary>
        /// 被关注用户
        /// </summary>
        [ForeignKey(nameof(FocusUser))]
        public Guid focusUserId { get; set; }
        public User FocusUser { get; set; }
    }
}
