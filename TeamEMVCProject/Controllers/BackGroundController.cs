//-----------------------------------------------【Function Indroduction】----------------------------------------------
//	  BackGround Controller：  background fundamental functions
//    Language：  C#
//    IDE：VS2013
//    2015.10.16  Created by RaymondMG  
//---------------------------------------------------------------------------------------------------------------------
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TeamEMVCProject.Models;

namespace TeamEMVCProject.Controllers
{
    public class BackGroundController : Controller
    {
        readonly DbHelper _db = new DbHelper();

        /// <summary>
        /// 更新资料库树状结构
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateInformationTree()
        {
            //存储上传图片文件
            var file = Request.Files["jsonfile"];
            if (file == null) return View("BackGround_Information");
            var filepath = Path.Combine(HttpContext.Server.MapPath("~/assets/Information/Json"), Path.GetFileName(file.FileName));
            file.SaveAs(filepath);

            return View("BackGround_Information");
        }



        
        //后台显示文章
        // GET: /BackGround/BackGround_ArticleDetail
        [AllowAnonymous]
        public ActionResult BackGround_ArticleDetail()
        {
            return View();
        }


        /// <summary>
        /// MarkDown编辑器发布文章
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult MarkDownArticlePublish(ArticleShare model)
        {
            if (model == null) return View("BackGround_Article");
            var article = new ArticleShare
            {
                ArticleName = model.ArticleName ?? "null",
                ArticleDescribe = model.ArticleDescribe ?? "null",
                ArticleInfo = model.ArticleInfo ?? "null",
                ArticlePublishTime = DateTime.Now,
                ArticleImgSrc = "assets/images/LOGO.png"
            };

            _db.ArticleShareModule.Add(article);
            _db.SaveChanges();

            return View("BackGround_Article");
        }

        /// <summary>
        /// 添加最新作品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddGameProduct(LastProductedModel model)
        {
            var newlastprod = new LastProductedModel();
            //存储上传图片文件
            var file = Request.Files["pic"];
            if(file!=null)
            {
                var filepath = Path.Combine(HttpContext.Server.MapPath("~/assets/images/Product"), Path.GetFileName(file.FileName));
                newlastprod.ProductImgSrc = "assets/images/Product/" + Path.GetFileName(file.FileName);
                file.SaveAs(filepath);
            }

            newlastprod.ProductName = model.ProductName;
            newlastprod.ProductDescribe = model.ProductDescribe;
            newlastprod.ProductPublishTime = DateTime.Now;


            _db.LastProductedModule.Add(newlastprod);
            _db.SaveChanges();

            return View("BackGround_GameProject");
        }

        //后台游戏项目
        // GET: /BackGround/BackGround_GameProject
        [AllowAnonymous]
        public ActionResult BackGround_GameProject()
        {
            return View();
        }

        //后台游戏项目
        // GET: /BackGround/BackGround_GameProject
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult EditGameProject(LastProductedModel model)
        {
            if (!ModelState.IsValid) return View("BackGround_GameProject");
            var nowEditModel = _db.LastProductedModule.FirstOrDefault(it => it.id == model.id);
            if (nowEditModel != null)
            {
                nowEditModel.ProductName = model.ProductName;
                nowEditModel.ProductPublishTime = DateTime.Now;
                nowEditModel.ProductDescribe = model.ProductDescribe;
                nowEditModel.ProductImgSrc = model.ProductImgSrc;
            }
            _db.SaveChanges();

            return View("BackGround_GameProject");
        }


        //后台游戏项目删除
        // GET: /BackGround/BackGround_GameProject
        [HttpPost]
        public ActionResult DeleteLastProductedModule(int deleteId)
        {
            _db.Entry(_db.LastProductedModule.FirstOrDefault(it => it.id == deleteId)).State = EntityState.Deleted;
            _db.SaveChanges();
            return View("BackGround_GameProject");
        }

        //后台资料信息
        // GET: /BackGround/BackGround_Information
        [AllowAnonymous]
        public ActionResult BackGround_Information()
        {
            return View();
        }

        //后台技术文章
        // GET: /BackGround/BackGround_Article
        [AllowAnonymous]
        public ActionResult BackGround_Article()
        {
            return View();
        }

        //后台数据管理
        // GET: /BackGround/BackGround_DataManager
        [AllowAnonymous]
        public ActionResult BackGround_DataManager()
        {
            return View();
        }

        //后台用户管理
        // GET: /BackGround/BackGround_UserManager
        [AllowAnonymous]
        public ActionResult BackGround_UserManager()
        {
            return View();
        }


        //后台用户管理删除
        // GET: /BackGround/BackGround_UserManager
        [HttpPost]
        public ActionResult DeleteLoginModule(int deleteId)
        {
            _db.Entry(_db.LoginModule.FirstOrDefault(it => it.Uid == deleteId)).State = EntityState.Deleted;
            _db.SaveChanges();
            return View("BackGround_UserManager");
        }
        
        //后台用户管理
        // GET: /BackGround/BackGround_UserManager
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult BackGround_UserManager(UserModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var nowEditModel = _db.LoginModule.FirstOrDefault(it => it.Uid == model.Uid);
            if (nowEditModel != null)
            {
                nowEditModel.UserName = model.UserName;
                nowEditModel.PassWord = model.PassWord;
            }
            _db.SaveChanges();

            return View(model);
        }

        //后台主页
        // GET: /BackGround/BackGround_Index
        [AllowAnonymous]
        public ActionResult BackGround_Index()
        {
            return View();
        }

        //后台注册
        // GET: /BackGround/BackGround_Register
        [AllowAnonymous]
        public ActionResult BackGround_Register()
        {
            return View();
        }

        //后台注册
        // GET: /BackGround/BackGround_Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult BackGround_Register(RegisterModel model)
        {
            if (!ModelState.IsValid) return View(model);
            // 接收用户注册
            try
            {
                if (_db.LoginModule.FirstOrDefault(it => it.UserName == model.UserName) != null)
                {
                    const string message = "用户名已经存在";
                    ViewBag.Msg = message;
                    return View();
                }
                var newUser = new UserModel
                {
                    UserName = model.UserName,
                    PassWord = model.Password
                };
                _db.LoginModule.Add(newUser);
                _db.SaveChanges();

                Session["UserName"] = model.UserName;
                return RedirectToAction("BackGround_Index", "BackGround");
            }
            catch (MembershipCreateUserException e)
            {
                   
            }

            return View(model);
        }

        //后台登陆
        // GET: /BackGround/BackGround_Login
        [AllowAnonymous]
        public ActionResult BackGround_Login()
        {
            return View();
        }

        //后台登陆
        // GET: /BackGround/BackGround_Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult BackGround_Login(UserModel model)
        {
            if (ModelState.IsValid && _db.LoginModule.FirstOrDefault(it => it.UserName == model.UserName && it.PassWord == model.PassWord) != null)
            {
                Session["UserName"] = model.UserName;
                return RedirectToAction("BackGround_Index", "BackGround");
            }

            ModelState.AddModelError("", "用户名或密码错误");
            return View(model);
        }

        /// <summary>
        /// 编辑文章
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult EditArticle(ArticleShare model)
        {
            //if (!ModelState.IsValid) return View("BackGround_Article");
            var nowEditModel = _db.ArticleShareModule.FirstOrDefault(it => it.id == model.id);
            //if (nowEditModel != null)
            //{
                nowEditModel.ArticleName = model.ArticleName;
                nowEditModel.ArticlePublishTime = DateTime.Now;
                nowEditModel.ArticleDescribe = model.ArticleDescribe;
                nowEditModel.ArticleImgSrc = model.ArticleImgSrc;
                nowEditModel.ArticleInfo = model.ArticleInfo;
            //}
            _db.SaveChanges();

            return View("BackGround_Article");
        }
        /// <summary>
        /// 删除文章
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteArticleModule(int deleteId)
        {
            _db.Entry(_db.ArticleShareModule.FirstOrDefault(it => it.id == deleteId)).State = EntityState.Deleted;
            _db.SaveChanges();
            return View("BackGround_Article");
        }
    }
}
