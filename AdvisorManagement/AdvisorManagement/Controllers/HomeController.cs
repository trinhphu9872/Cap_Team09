using AdvisorManagement.Middleware;
using AdvisorManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace AdvisorManagement.Controllers
{
  
    public class HomeController : Controller
    {
        private CP25Team09Entities db = new CP25Team09Entities();
        private AccountMiddleware accountService = new AccountMiddleware();
        public  ActionResult Index()
        {
            string SessionRe = "";
            string user_mail = User.Identity.Name;

            if (user_mail != null && user_mail != "")
            {
                var sql = db.AccountUser.FirstOrDefault(x => x.email == user_mail);
                if (sql == null)
                {
                    // get Claims
                    //StudentClass student = (StudentClass)(accountService.getOrgMail((ClaimsIdentity) User.Identity));
                    AccountUser userNew = new AccountUser();
                    userNew.ID = Guid.NewGuid();
                    userNew.email = user_mail;
                    userNew.createtime = DateTime.Now;
                    userNew.user_code = "";//student.user_code;
                    userNew.username = ""; //student.user_name;
                    userNew.id_Role = 1;
                    //userNew.user_code =1
                    db.AccountUser.Add(userNew);
                    db.SaveChanges();
                    SessionRe = user_mail;
                }
                else
                {
                    SessionRe = sql.email;
                }
            }
       
            return View();
        }
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}