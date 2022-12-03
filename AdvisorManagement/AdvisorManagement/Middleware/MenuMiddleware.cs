using AdvisorManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdvisorManagement.Middleware
{
    public class MenuMiddleware
    {
        private CP25Team09Entities db = new CP25Team09Entities();

        public object getMenu(string userMail)
        {
            var seeMenu = (from pq in db.AccountUser
                           join mnrole in db.RoleMenu on pq.id_Role equals mnrole.id_Role
                           join mn in db.Menu on mnrole.id_Menu equals mn.id
                           where pq.email == userMail && pq.id_Role == mnrole.id_Role
                           select new Models.ViewModel.MenuItem
                           {
                               menuName = mn.nameMenu
                           }).ToList();
            return seeMenu;
        }
    }
}