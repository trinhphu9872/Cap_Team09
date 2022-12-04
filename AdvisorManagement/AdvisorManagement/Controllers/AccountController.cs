using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security;
using AdvisorManagement.Models;
using System.IO;
using System.Data.Entity;
using CapstoneProjectTeam09.model;

namespace AdvisorManagement.Controllers
{
    public class AccountController : Controller
    {
        CP25Team09Entities model = new CP25Team09Entities();
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
        public ActionResult UserProfile()
        {
            User user1 = new User();
            string id = "605DE908-6C1B-416A-B6C7-34493C0235F7";
            var user = model.AccountUsers.Find(Guid.Parse(id));


            user1.MSSV = user.user_code;
            user1.name = user.username;
            user1.phoneNumber = user.phone;
            user1.email = user.email;
            user1.dateOfBirth = user.dateofbirth;
            user1.image = user.picture;
            return View(user1);

            return View();
        }
        [HttpPost]
        public ActionResult UserProfile(string fullName, string MSSV, string email, string phoneNumber, string dateOfBirth, User user2)
        {
            string id = "605DE908-6C1B-416A-B6C7-34493C0235F7";
            var user = model.AccountUsers.Find(Guid.Parse(id));

            user.username = fullName;
            user.phone = phoneNumber;
            user.dateofbirth = DateTime.Parse(dateOfBirth);
            if (user2.ImageUpload != null)
            {
                string filename = Path.GetFileNameWithoutExtension(user2.ImageUpload.FileName).ToString();
                string extension = Path.GetExtension(user2.ImageUpload.FileName);
                filename = filename + extension;
                user.picture = "~/Image/imageProfile/" + filename;
                user2.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Image/imageProfile/"), filename));
            }
            model.Entry(user).State = EntityState.Modified;
            model.SaveChanges();
            User user1 = new User();
            user1.MSSV = user.user_code;
            user1.name = user.username;
            user1.phoneNumber = user.phone;
            user1.email = user.email;
            user1.dateOfBirth = user.dateofbirth;
            user1.image = user.picture;
            return View(user1);
        }
        public ActionResult IndexManagementAccount()
        {
            var listAccount = model.AccountUsers.ToList();

            //var listrole = model.Roles.Select(e => new { e.MaTinhTP, e.TenTinhTP }).Distinct().ToList();
            //listrole.Insert(0, new { id = 0, roleName = "-- Chọn quyền --" });
            //ViewBag.listroles = new SelectList(listrole, "id", "roleName");
            return View(listAccount);
        }

        public ActionResult EditAccount(Guid id)
        {
            var thongtin = model.AccountUsers.Find(id);

            //var listrole = model.Roles.ToList();
            //List<string> role = new List<string>();
            //role.Add("---- Chọn Quyền----");
            //foreach (var Role in listrole)
            //{
            //    role.Add(Role.roleName);
            //}
            //ViewBag.Listroles = new SelectList(role);
            var listrole = model.Roles.Select(e => new { e.id, e.roleName }).Distinct().ToList();
            listrole.Insert(0, new { id = 0, roleName = "-- Chọn quyền --" });
            ViewBag.Listroles = new SelectList(listrole, "id", "roleName");

            return View(thongtin);
        }
        [HttpPost]
        public ActionResult EditAccount(AccountUser user, int Listroles)
        {

            if (user.ImageUpload != null)
            {
                string filename = Path.GetFileNameWithoutExtension(user.ImageUpload.FileName).ToString();
                string extension = Path.GetExtension(user.ImageUpload.FileName);
                filename = filename + extension;
                user.picture = "~/Image/imageProfile/" + filename;
                user.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Image/imageProfile/"), filename));
            }
            var role = model.Roles.Find(Listroles);
            user.id_Role = Listroles;

            model.Entry(user).State = EntityState.Modified;
            model.SaveChanges();
            return RedirectToAction("IndexManagementAccount", "Account");
        }
    }
}
