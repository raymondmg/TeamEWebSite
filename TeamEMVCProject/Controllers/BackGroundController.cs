//-----------------------------------------------【Function Indroduction】----------------------------------------------
//	  BackGround Controller：  background fundamental functions
//    Language：  C#
//    IDE：VS2013
//    2015.10.16  Created by RaymondMG  
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Web.WebPages.OAuth;
using MVCEF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TeamEMVCProject.Models;

namespace TeamEMVCProject.Controllers
{
    public class BackGroundController : Controller
    {

        DbHelper db = new DbHelper();

        //后台游戏项目
        // GET: /BackGround/BackGround_GameProject
        [AllowAnonymous]
        public ActionResult BackGround_GameProject()
        {
            return View();
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
                    if (db.LoginModule.Where(it => it.UserName == model.UserName).FirstOrDefault() != null)
                    {
                        Message = "用户名已经存在";
                        ViewBag.Msg = Message;
                        return View();
                    }
                    LoginModel newUser = new LoginModel();
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
        public ActionResult BackGround_Login(LoginModel model)
        {
            if (ModelState.IsValid && db.LoginModule.Where(it => it.UserName == model.UserName && it.PassWord == model.PassWord).FirstOrDefault() != null)
            {
                Session["UserName"] = model.UserName;
                return RedirectToAction("BackGround_Index", "BackGround");
            }

            ModelState.AddModelError("", "用户名或密码错误");
            return View(model);
        }

      

    }
}
