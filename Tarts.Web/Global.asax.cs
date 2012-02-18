using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NHibernate;
using NHibernate.Context;
using Tarts.Persistance;
using Tarts.Web.Areas.TartsAdmin.Controllers.Base;

namespace Tarts.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Content/{*pathInfo}");
            routes.IgnoreRoute("Scripts/{*pathInfo}");
            //routes.MapRoute(
            //    "Default", // Route name
            //    "{controller}/{action}/{id}", // URL with parameters
            //    new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            //);
            routes.MapRoute("Home","", new { controller = "Home", action = "Index" });
            routes.MapRoute("FBLogin","Facebooklogin", new { controller = "FacebookLogin", action = "Index" });
            routes.MapRoute("Account","Account/{action}",new { controller = "Account", Action = "Index" });
            routes.MapRoute("Galleries", "Galleries", new { controller = "Galleries", Action = "Index" });
            routes.MapRoute("GalleryView", "Galleries/{id}", new { controller = "Galleries", Action = "View", id = UrlParameter.Optional });
            routes.MapRoute("Events", "Events", new { controller = "Events", Action = "Index" });
            routes.MapRoute("EventView", "Events/{id}", new { controller = "Events", Action = "View", id = UrlParameter.Optional });
            routes.MapRoute("EventLineup", "Events/{id}/Lineup", new { controller = "Events", Action = "Lineup", id = UrlParameter.Optional });
            routes.MapRoute("EventTickets", "Events/{id}/Tickets", new { controller = "Events", Action = "Tickets", id = UrlParameter.Optional });
            routes.MapRoute("BookingStage1", "Bookings/{eventName}/{ticketID}/CustomerAccount", new { controller = "Booking", Action = "CustomerAccount", eventName = UrlParameter.Optional, ticketID = UrlParameter.Optional });
            routes.MapRoute("BookingLogin", "Bookings/{eventName}/{ticketID}/AccountLogin", new { controller = "Booking", Action = "AccountLogin", eventName = UrlParameter.Optional, ticketID = UrlParameter.Optional });
            routes.MapRoute("ReservationSummary", "Bookings/ReservationSummary/{id}", new { controller = "Booking", Action = "ReservationSummary",id = UrlParameter.Optional, bookingID = UrlParameter.Optional });
            routes.MapRoute("MakePayment", "Bookings/MakePayment/{id}", new { controller = "Booking", Action = "MakePayment", id = UrlParameter.Optional, bookingID = UrlParameter.Optional });
            routes.MapRoute("PaymentResult", "Payments/PaymentResult", new { controller = "Payments", Action = "PaymentResult" });
            routes.MapRoute("SoldOut", "Bookings/SoldOut", new { controller = "Booking", Action = "SoldOut" });
            routes.MapRoute("ArtistBio", "Artists/Bio/{id}", new { controller = "Artists", Action = "Bio", id = UrlParameter.Optional});
            routes.MapRoute("Page", "{*slug}", new { controller = "Page", action = "ChoosePage" });



        }

        private ITransaction NHHibernateTransaction;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            ControllerBuilder.Current.DefaultNamespaces.Add("Tarts.Web.Controllers");
            RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalFilters.Filters.Add(new GlobalActionFilter());
            GlobalFilters.Filters.Add(new GlobalAdminActionFilter());
            RegisterRoutes(RouteTable.Routes);
        }

        public void Application_BeginRequest(object sender, EventArgs e)
        {
            DBHelper.BronsonDBManager.OpenConnection();
            CurrentSessionContext.Bind(DBHelper.NHibernateSessionFactory.OpenSession());
            //DBHelper.Abort = false;
            //DBHelper.BeginTransaction();
            NHHibernateTransaction = DBHelper.NHibernateSessionFactory.GetCurrentSession().BeginTransaction();
        }


        private void Application_EndRequest(object sender, EventArgs e)
        {
            //if (DBHelper.Abort) DBHelper.Rollback(); 
            //else DBHelper.Commit();
            DBHelper.BronsonDBManager.CloseConnection();

            if (! DBHelper.Abort) NHHibernateTransaction.Commit();
            else NHHibernateTransaction.Rollback();
            
            CurrentSessionContext.Unbind(DBHelper.NHibernateSessionFactory);

        }
    }
}