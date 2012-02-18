using System;
using System.Web.Mvc;
using StructureMap;
using Tarts.Config;
using Tarts.Customers;
using Tarts.Persistance;

namespace Tarts.Web.Areas.TartsAdmin.Controllers.Base
{
    public class GlobalActionFilter : ActionFilterAttribute
    {
        public  GenericRepo Repo
        {
            get {return ObjectFactory.GetInstance<GenericRepo>();}
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var ctrl = (Controller)filterContext.Controller;
            if (ctrl.TempData["Message"] != null)
                ctrl.ViewBag.Message = ctrl.TempData["Message"];
            if (ctrl.TempData["ErrorMessage"] != null)
                ctrl.ViewBag.ErrorMessage = ctrl.TempData["ErrorMessage"];
            LoadCustomer(filterContext);
        }


        private void LoadCustomer(ActionExecutingContext filterContext)
        {
            var ctrl = (Controller)filterContext.Controller;
            var usrCookie = ctrl.Request.Cookies["TartsUser"];
           
            if (usrCookie != null)
            {
                try
                {
                    var user = Repo.GetById<Customer>(usrCookie.Value.DecryptInteger());
                    if (user != null)
                        ctrl.ViewBag.Customer = user;
                }
                catch  { }
            }
            

            ctrl.ViewBag.SiteSettings = Repo.GetById<Settings>(1);
        }
       
    }
}