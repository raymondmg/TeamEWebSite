using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamEMVCProject.Models
{
    /// <summary>
    /// 邮件发送
    /// </summary>
    public class MailPostInfo
    {
        public string MailName { get; set; }

        public string MailEmail { get; set; }

        public string MailContent { get; set; }
    }
}