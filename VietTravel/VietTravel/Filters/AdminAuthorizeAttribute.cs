using System;
using System.Web;
using System.Web.Mvc;

namespace VietTravel.Filters
{
    public class AdminAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["user"] == null)
            {
                filterContext.Result = new RedirectResult("~/Admin/Account/AdminLogin");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
