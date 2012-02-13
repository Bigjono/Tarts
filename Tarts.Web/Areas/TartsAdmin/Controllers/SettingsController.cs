using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tarts.Config;
using Tarts.Persistance;
using Tarts.Web.Areas.TartsAdmin.Models.Settings;

namespace Tarts.Web.Areas.TartsAdmin.Controllers
{
    public class SettingsController : Controller
    {

        private GenericRepo Repo;
        public Settings Settings { get; set; }

        public SettingsController(GenericRepo repo)
        {
            Repo = repo;
            Settings = repo.GetById<Settings>(1) ?? new Settings();
        }

        [Authorize]
        public ActionResult Index()
        {
            return View(Settings);
        }

        [Authorize, HttpPost]
        public ActionResult SEOSettings(string AnalyticsTrackingCode)
        {
            Settings.UpdateSEOSettings(AnalyticsTrackingCode);
            Repo.Save(Settings);
            TempData.Add("Message","SEO Settings Updated");
            return RedirectToAction("Index");
        }

        [Authorize, HttpPost]
        public ActionResult EmailSettings(EmailSettingsPostModel model)
        {
            Settings.UpdateEmailSettings(model.SmtpHost,model.SmtpUsername,model.SmtpPassword,model.EmailFromName,model.EmailFromAddress,model.ForceEmailsTo);
            Repo.Save(Settings);
            TempData.Add("Message", "Email Settings Updated");
            return RedirectToAction("Index");
        }

        [Authorize, HttpPost]
        public ActionResult PaymentSettings(PaymentSettingsPostModel model)
        {
            Settings.UpdatePaymentSettings(model.PaypalUrl,model.PaypalPdtToken,model.PaypalUsername);
            Repo.Save(Settings);
            TempData.Add("Message", "Payment Settings Updated");
            return RedirectToAction("Index");
        }
    }
}
