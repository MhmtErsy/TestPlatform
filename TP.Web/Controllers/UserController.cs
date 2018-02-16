using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TP.BusinessLayer.RoleManagers;

namespace TP.Web.Controllers
{
    public class UserController : Controller
    {
        TesterManager TeM = new TesterManager();
        TestMasterManager TeMM = new TestMasterManager();
        CustomerManager CuM = new CustomerManager();
        AdminManager AdM = new AdminManager();
        // GET: User
        public ActionResult TesterList()
        {
            return View(TeM.List());
        }
        public ActionResult TestMasterList()
        {
            return View(TeMM.List());
        }
        public ActionResult CustomerList()
        {
            return View(CuM.List());
        }
        public ActionResult AdminList()
        {
            return View(AdM.List());
        }
    }
}