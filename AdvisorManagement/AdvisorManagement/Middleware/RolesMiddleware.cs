using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdvisorManagement.Middleware
{
    public class AdminRoles : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["user_mail"] == null || filterContext.HttpContext.Session["Role"].ToString() != "Admin")
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }
        }
    }
    public class StudentRoles : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["user_mail"] == null || filterContext.HttpContext.Session["Role"].ToString() != "Student")
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }
        }
    }
    public class AdvisorRoles : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["user_mail"] == null || filterContext.HttpContext.Session["Role"].ToString() != "Advisor")
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }
        }
    }
}