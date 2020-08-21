using System.Web.Mvc;
using System.Web.Routing;

namespace qlkdst.Common
{
    public class BaseController : Controller
    {


        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // var session = (user)Session[CommonConstants.USER_SESSION];

            if (Session["username"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Login", action = "Index", area = "" }));
            }
            base.OnActionExecuting(filterContext);
        }



        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
    }
}