using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogSystem.Models
{
    /// <summary>
    /// 用户表
    /// </summary>
    public class User:BaseEntity
    {
        [Required, StringLength(40),Column(TypeName ="varchar")]
        public string Email { get; set; }
        [Required, Column(TypeName = "varchar"), StringLength(300)]
        public string PassWord { get; set; }
        [Required, StringLength(300), Column(TypeName ="varchar")]
        public string ImagePath { get; set; }
        /// <summary>
        /// 粉丝数量
        /// </summary>
        public int FansCount { get; set; }
        /// <summary>
        /// 关注数
        /// </summary>
        public int FocusCount { get; set; }
        /// <summary>
        /// 网站名称
        /// </summary>
        public string SiteName { get; set; }
        [StringLength(50)]
        [Column(TypeName ="nvarchar")]
        public string NickName { get; set; }

    }
}
