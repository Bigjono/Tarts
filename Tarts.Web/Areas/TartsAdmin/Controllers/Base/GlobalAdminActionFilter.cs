using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tarts.Web.Areas.TartsAdmin.Controllers.Base
{
    public class GlobalAdminActionFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var ctrl = (Controller)filterContext.Controller;
            if (ctrl.TempData["Message"] != null)
                ctrl.ViewBag.Message = ctrl.TempData["Message"];
            if (ctrl.TempData["ErrorMessage"] != null)
                ctrl.ViewBag.ErrorMessage = ctrl.TempData["ErrorMessage"];


        }
    }
}