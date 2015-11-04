//-----------------------------------------------【Function Indroduction】----------------------------------------------
//	  Home Controller：  fundamental functions
//    Language：  C#
//    IDE：VS2013
//    2015.10.16  Created by RaymondMG  
//---------------------------------------------------------------------------------------------------------------------

using System;
using System.Net.Mail;
using System.Web.Mvc;
using TeamEMVCProject.Models;

namespace TeamEMVCProject.Controllers
{
    public class HomeController : Controller
    {

        //
        // GET: /Home/

        public ActionResult Index()
        {

                return View();
        }

        public ActionResult LastedProduect()
        {
            return View();
        }

        public ActionResult TeamShare()
        {
            return View();
        }

        public ActionResult Article()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(MailPostInfo mailModel)
        {
            var msg = new MailMessage();
            //收件人
            msg.To.Add("414505667@qq.com");


            msg.From = new MailAddress("1506493872@qq.com", "发件人", System.Text.Encoding.UTF8);


            msg.Subject = "邮箱"+mailModel.MailEmail+"用户:"+mailModel.MailName+"发送的消息";//邮件标题 
            msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码 
            msg.Body = "邮件内容为："+mailModel.MailContent;//邮件内容 
            msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码 
            msg.IsBodyHtml = false;//是否是HTML邮件 
            msg.Priority = MailPriority.High;//邮件优先级


            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("1506493872@qq.com", "lttwx1994515");
            //注册的邮箱和密码 
            client.Host = "smtp.qq.com";
            object userState = msg;
   
            client.Send(msg);

            return View();
        }
    }
}
