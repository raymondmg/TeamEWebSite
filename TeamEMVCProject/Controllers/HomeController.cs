using MVCEF.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TeamEWebSite.Controllers
{
    public class HomeController : Controller
    {

      
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult edit(string id)
        //{
        //    var stu = db.Users.Where(it => it.Num == id).FirstOrDefault();
        //    return View(stu);
        //}
        //[HttpPost]
        //public ActionResult edit(UserInfo stu, FormCollection form, string id)
        //{
        //    stu.Num = id;
        //    db.Entry(stu).State = EntityState.Modified;
        //    db.SaveChanges();
        //    return RedirectToAction("Index", "Home");
        //}
        ////删除
        //public ActionResult Del(string id)
        //{
        //    /*方法1
        //    var stu = db.Students.Where(it => it.Num == id).FirstOrDefault();
        //    db.Entry(stu).State = EntityState.Deleted;
        //    db.SaveChanges();
        //     */
        //    //方法2
        //    string strsql = "delete from tb_Students where Num=@Num";
        //    SqlParameter[] paras =
        //   {
        //       new SqlParameter("@Num",SqlDbType.NVarChar,128)
        //   };
        //    paras[0].Value = id;
        //    db.Database.ExecuteSqlCommand(strsql, paras);
        //    db.SaveChanges();
        //    return RedirectToAction("Index", "Home");
        //}


        public ActionResult LastedProduect()
        {
            return View();
        }

        public ActionResult TeamShare()
        {
            return View();
        }
    }
}
