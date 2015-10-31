using System;
using System.Web.Mvc;

namespace TeamEMVCProject.Models
{
    /// <summary>
    /// 用户类
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// 每个浏览器对应的ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 用户账号ID
        /// </summary>
        public Guid AccountID { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string Picture { get; set; }

        /// <summary>
        /// 用户的未读消息数目
        /// </summary>
        public int UnReadMsgCount { get; set; }
    }

    /// <summary>
    /// 用户聊天信息
    /// </summary>
    public class ChatContent
    {
        /// <summary>
        /// 构造信息和显示信息的头像
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="img"></param>
        public ChatContent(string msg, string img)
        {
            Content = msg;
            imgSrc = img;
        }

        /// <summary>
        /// 显示信息的头像
        /// </summary>
        public string imgSrc { get; set; }

        /// <summary>
        /// 浏览器对应的ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 用户账号ID
        /// </summary>
        public Guid AccountID { get; set; }
        /// <summary>
        /// 聊天内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 聊天创建的时间
        /// </summary>
        public DateTime CreateDateTime { get; set; }
        /// <summary>
        /// 可选，是否发给指定的用户
        /// </summary>
        public Guid AtID { get; set; }

    }
}
