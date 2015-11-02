﻿//-----------------------------------------------【Function Indroduction】----------------------------------------------
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
        readonly DbHelper db = new DbHelper();

        /// <summary>
        /// 更新资料库树状结构
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateInformationTree()
        {
            //存储上传图片文件
            HttpPostedFileBase file = Request.Files["jsonfile"];
            if (file != null)
            {
                string filepath = Path.Combine(HttpContext.Server.MapPath("~/assets/Information/Json"), Path.GetFileName(file.FileName));
                file.SaveAs(filepath);
            }

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

            db.ArticleShareModule.Add(article);
            db.SaveChanges();

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
            LastProductedModel newlastprod = new LastProductedModel();
            //存储上传图片文件
            HttpPostedFileBase file = Request.Files["pic"];
            if(file!=null)
            {
                string filepath = Path.Combine(HttpContext.Server.MapPath("~/assets/images/Product"), Path.GetFileName(file.FileName));
                newlastprod.ProductImgSrc = "assets/images/Product/" + Path.GetFileName(file.FileName);
                file.SaveAs(filepath);
            }

            newlastprod.ProductName = model.ProductName;
            newlastprod.ProductDescribe = model.ProductDescribe;
            newlastprod.ProductPublishTime = DateTime.Now;


            db.LastProductedModule.Add(newlastprod);
            db.SaveChanges();

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
            if (ModelState.IsValid)
            {
                LastProductedModel nowEditModel = db.LastProductedModule.FirstOrDefault(it => it.id == model.id);
                nowEditModel.ProductName = model.ProductName;
                nowEditModel.ProductPublishTime = DateTime.Now;
                nowEditModel.ProductDescribe = model.ProductDescribe;
                nowEditModel.ProductImgSrc = model.ProductImgSrc;
                db.SaveChanges();
            }

            return View("BackGround_GameProject");
        }


        //后台游戏项目删除
        // GET: /BackGround/BackGround_GameProject
        [HttpPost]
        public ActionResult DeleteLastProductedModule(int deleteId)
        {
            db.Entry(db.LastProductedModule.FirstOrDefault(it => it.id == deleteId)).State = EntityState.Deleted;
            db.SaveChanges();
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
            db.Entry(db.LoginModule.FirstOrDefault(it => it.Uid == deleteId)).State = EntityState.Deleted;
            db.SaveChanges();
            return View("BackGround_UserManager");
        }
        
        //后台用户管理
        // GET: /BackGround/BackGround_UserManager
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult BackGround_UserManager(UserModel model)
        {
            if (ModelState.IsValid )
            {
               UserModel nowEditModel = db.LoginModule.FirstOrDefault(it => it.Uid == model.Uid);
               nowEditModel.UserName = model.UserName;
               nowEditModel.PassWord = model.PassWord;              
                db.SaveChanges();
            }

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
            if (ModelState.IsValid)
            {
                // 接收用户注册
                try
                {
                    string Message = string.Empty;
                    if (db.LoginModule.FirstOrDefault(it => it.UserName == model.UserName) != null)
                    {
                        Message = "用户名已经存在";
                        ViewBag.Msg = Message;
                        return View();
                    }
                    UserModel newUser = new UserModel();
                    newUser.UserName = model.UserName;
                    newUser.PassWord = model.Password;
                    db.LoginModule.Add(newUser);
                    db.SaveChanges();

                    Session["UserName"] = model.UserName;
                    return RedirectToAction("BackGround_Index", "BackGround");
                }
                catch (MembershipCreateUserException e)
                {
                   
                }
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
            if (ModelState.IsValid && db.LoginModule.FirstOrDefault(it => it.UserName == model.UserName && it.PassWord == model.PassWord) != null)
            {
                Session["UserName"] = model.UserName;
                return RedirectToAction("BackGround_Index", "BackGround");
            }

            ModelState.AddModelError("", "用户名或密码错误");
            return View(model);
        }



    }
}
