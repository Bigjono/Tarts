using System.Web.Mvc;

namespace Tarts.Web.Areas.TartsAdmin
{
    public class TartsAdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "TartsAdmin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "TartsAdmin_default",
                "TartsAdmin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
