using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security;
using AdvisorManagement.Models;
using AdvisorManagement.Middleware;
using AdvisorManagement.Models.ViewModel;
using System.Security.Claims;

namespace AdvisorManagement.Controllers
{
   

    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }


        public void SignIn()
        {
            // Send an OpenID Connect sign-in request.
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        public void SignOut()
        {
            string callbackUrl = Url.Action("SignOutCallback", "Account", routeValues: null, protocol: Request.Url.Scheme);

            HttpContext.GetOwinContext().Authentication.SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        }

        public ActionResult SignOutCallback()
        {
            if (Request.IsAuthenticated)
            {
                // Redirect to home page if the user is authenticated.
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}


//string user_mail = User.Identity.Name;

//var sql = db.AccountUser.FirstOrDefault(x => x.email == user_mail);
//if (sql == null)
//{
//    // get Claims
//    //StudentClass student = (StudentClass)(accountService.getOrgMail((ClaimsIdentity) User.Identity));
//    AccountUser userNew = new AccountUser();
//    userNew.ID = Guid.NewGuid();
//    userNew.email = user_mail;
//    userNew.createtime = DateTime.Now;
//    userNew.user_code = "";//student.user_code;
//    userNew.username = ""; //student.user_name;
//    userNew.id_Role = 1;
//    //userNew.user_code =1
//    db.AccountUser.Add(userNew);
//    db.SaveChanges();
//    Session["userName"] = user_mail;
//}
//else
//{
//    Session["userName"] = sql.email;
//}
// Redirect to home page if the user is authenticated.