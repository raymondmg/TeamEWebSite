//-----------------------------------------------【Function Indroduction】----------------------------------------------
//	  Home Controller：  fundamental functions
//    Language：  C#
//    IDE：VS2013
//    2015.10.16  Created by RaymondMG  
//---------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
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
    }
}
