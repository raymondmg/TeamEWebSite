using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

///后台模块
namespace TeamEMVCProject.Models
{
    /// <summary>
    /// 登录模块
    /// </summary>
    [Table("T_UserInfo", Schema = "dbo")]//关联数据表
    public class LoginModel
    {
        [Key]
        public int uid { get; set; }

        //用户名
        [MaxLength(10), Required(ErrorMessage = "用户名不能为空")]
        [Column(TypeName = "nvarchar")]
        public string UserName { get; set; }
        //密码
        [MaxLength(10), Required(ErrorMessage = "密码不能为空")]
        [Column(TypeName = "nvarchar")]
        public string PassWord { get; set; }
    }

    /// <summary>
    /// 注册模块
    /// </summary>
    [Table("T_Register", Schema = "dbo")]//关联数据表
    public class RegisterModel
    {
        [Key]
        public int id { get; set; }

        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "字符长度最少为6", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码与确认密码不匹配")]
        public string ConfirmPassword { get; set; }
    }

     /// <summary>
    /// 近期作品模块
    /// </summary>
    [Table("T_LastProductedDisplay", Schema = "dbo")]//关联数据表
    public class LastProductedModel
    {
        [Key]
        public int id { get; set; }

        [Required]
        [Display(Name = "作品名称")]
        public string ProductName { get; set; }

        [Required] 
        [Display(Name = "作品发表时间")]
        public DateTime ProductPublishTime { get; set; }
     
        [Required] 
        [Display(Name = "作品介绍")]
        public string ProductDescribe { get; set; }

        [Required] 
        [Display(Name = "作品展示截图地址")]
        public string ProductImgSrc { get; set; }
    }

    /// <summary>
    /// 技术文章模块
    /// </summary>
    [Table("T_ArticleShare", Schema = "dbo")]//关联数据表
    public class ArticleShare
    {
        [Key]
        public int id { get; set; }

        [Required]
        [Display(Name = "文章名称")]
        public string ArticleName { get; set; }

        [Required]
        [Display(Name = "文章发表时间")]
        public DateTime ArticlePublishTime { get; set; }

        [Required]
        [Display(Name = "文章介绍")]
        public string ArticleDescribe { get; set; }

        [Required]
        [Display(Name = "文章展示截图地址")]
        public string ArticleImgSrc { get; set; }

        [Required]
        [Display(Name = "文章内容")]
        public string ArticleInfo { get; set; }
    }
  
}