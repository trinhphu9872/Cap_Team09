using AdvisorManagement.Middleware;
using AdvisorManagement.Models;
using AdvisorManagement.Models.ViewModel;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web.WebPages;

namespace AdvisorManagement.Controllers
{
  
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private CP25Team09Entities dbApp = new CP25Team09Entities();
        private AccountMiddleware accountService = new AccountMiddleware();
        private MenuMiddleware serviceMenu = new MenuMiddleware();
        private string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private string appKey = ConfigurationManager.AppSettings["ida:ClientSecret"];
        private string aadInstance = EnsureTrailingSlash(ConfigurationManager.AppSettings["ida:AADInstance"]);
        private string graphResourceID = "https://graph.windows.net";

        public async Task<ActionResult> Index()
        {
        
            string SessionRe = "";
            int roles = 3;
            string user_mail = User.Identity.Name;
            if (user_mail != null && user_mail != "")
            {
                var sql = dbApp.AccountUser.FirstOrDefault(x => x.email == user_mail);
                if (sql == null)
                {
                    string tenantID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
                    string userObjectID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
                    Uri servicePointUri = new Uri(graphResourceID);
                    Uri serviceRoot = new Uri(servicePointUri, tenantID);
                    ActiveDirectoryClient activeDirectoryClient = new ActiveDirectoryClient(serviceRoot,
                          async () => await GetTokenForApplication());

                    // use the token for querying the graph to get the user details

                    var result = await activeDirectoryClient.Users
                        .Where(u => u.ObjectId.Equals(userObjectID))
                        .ExecuteAsync();
                    IUser user = result.CurrentPage.ToList().First();
                    AccountUser userNew = new AccountUser();
                    if (user.CompanyName == "VLU" &&  user.PhysicalDeliveryOfficeName == "Trường Đại học Văn Lang")
                    {
                        roles = 2;
                        userNew.phone = user.Mobile;

                    }
                    userNew.id_Role = roles;
                    // get Claims
                    StudentClass student = (StudentClass)(accountService.UserProfile(user.DisplayName,roles));
                    
                    userNew.ID = Guid.NewGuid();
                    userNew.email = user_mail;
                    userNew.createtime = DateTime.Now;
                    userNew.user_code = student.user_code;
                    userNew.username =student.user_name;
           
                    //userNew.user_code =1
                    dbApp.AccountUser.Add(userNew);
                    dbApp.SaveChanges();
                    SessionRe = user_mail;
                }
                else
                {
                    SessionRe = sql.email;
                }
                var seeMenu = serviceMenu.getMenu(user_mail);
              
                ViewBag.menu = seeMenu;
                
                return View(seeMenu);
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

        public ActionResult ThongBao()
        {
            return View();
        }

        public async Task<string> GetTokenForApplication()
        {
            string signedInUserID = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
            string tenantID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            string userObjectID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            // get a token for the Graph without triggering any user interaction (from the cache, via multi-resource refresh token, etc)
            ClientCredential clientcred = new ClientCredential(clientId, appKey);
            // initialize AuthenticationContext with the token cache of the currently signed in user, as kept in the app's database
            AuthenticationContext authenticationContext = new AuthenticationContext(aadInstance + tenantID, new ADALTokenCache(signedInUserID));
            AuthenticationResult authenticationResult = await authenticationContext.AcquireTokenSilentAsync(graphResourceID, clientcred, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));
            return authenticationResult.AccessToken;
        }

        private static string EnsureTrailingSlash(string value)
        {
            if (value == null)
            {
                value = string.Empty;
            }

            if (!value.EndsWith("/", StringComparison.Ordinal))
            {
                return value + "/";
            }

            return value;
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleMenu roleMenu = dbApp.RoleMenu.Find(id);
            if (roleMenu == null)
            {
                return HttpNotFound();
            }
            return View(roleMenu);
        }

        // GET: RoleMenus/Create
        public ActionResult Create()
        {
            ViewBag.id_Menu = new SelectList(dbApp.Menu, "id", "nameMenu");
            ViewBag.id_Role = new SelectList(dbApp.Role, "id", "roleName");
            return View();
        }

        // POST: RoleMenus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_Role,id_Menu")] RoleMenu roleMenu)
        {
            if (ModelState.IsValid)
            {
                dbApp.RoleMenu.Add(roleMenu);
                dbApp.SaveChanges();
                return RedirectToAction("RoleMenu");
            }

            ViewBag.id_Menu = new SelectList(dbApp.Menu, "id", "nameMenu", roleMenu.id_Menu);
            ViewBag.id_Role = new SelectList(dbApp.Role, "id", "roleName", roleMenu.id_Role);
            return View(roleMenu);
        }

        // GET: RoleMenus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleMenu roleMenu = dbApp.RoleMenu.Find(id);
            if (roleMenu == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_Menu = new SelectList(dbApp.Menu, "id", "nameMenu", roleMenu.id_Menu);
            ViewBag.id_Role = new SelectList(dbApp.Role, "id", "roleName", roleMenu.id_Role);
            return View(roleMenu);
        }

        // POST: RoleMenus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_Role,id_Menu")] RoleMenu roleMenu)
        {
            if (ModelState.IsValid)
            {
                dbApp.Entry(roleMenu).State = EntityState.Modified;
                dbApp.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_Menu = new SelectList(dbApp.Menu, "id", "nameMenu", roleMenu.id_Menu);
            ViewBag.id_Role = new SelectList(dbApp.Role, "id", "roleName", roleMenu.id_Role);
            return View(roleMenu);
        }

        // GET: RoleMenus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleMenu roleMenu = dbApp.RoleMenu.Find(id);
            if (roleMenu == null)
            {
                return HttpNotFound();
            }
            return View(roleMenu);
        }

        // POST: RoleMenus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoleMenu roleMenu = dbApp.RoleMenu.Find(id);
            dbApp.RoleMenu.Remove(roleMenu);
            dbApp.SaveChanges();
            return RedirectToAction("RoleMenu");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbApp.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}