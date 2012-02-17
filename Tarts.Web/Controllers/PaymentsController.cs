using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Mvc;
using Facebook.Web;
using Tarts.Config;
using Tarts.Ecommerce;
using Tarts.Persistance;
using Tarts.Web.Infrastructure.Helpers;
using Tarts.Web.Models.Payments;

namespace Tarts.Web.Controllers
{
    public class PaymentsController : Controller
    {
        private GenericRepo Repo;
        
        public PaymentsController(GenericRepo repo)
        {
            Repo = repo;
        }
        public ActionResult PaymentResult()
        {
            PaymentResultViewModel view = new PaymentResultViewModel();
            Settings cfg = ViewBag.SiteSettings;
            string txToken, query;
            string strResponse;
            txToken = Request.QueryString.Get("tx");
            query = string.Format("cmd=_notify-synch&tx={0}&at={1}", txToken, cfg.PaypalPdtToken);

            // Create the request back
            var req = (HttpWebRequest)WebRequest.Create(cfg.PaypalUrl);

            // Set values for the request back
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = query.Length;

            // Write the request back IPN strings
            var stOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
            stOut.Write(query);
            stOut.Close();

            // Do the request to PayPal and get the response
            StreamReader stIn = new StreamReader(req.GetResponse().GetResponseStream());
            strResponse = stIn.ReadToEnd();
            stIn.Close();

            // sanity check
            //mergeDictionary.Add("reponse", strResponse);
            
            // If response was SUCCESS, parse response string and output details
            if (strResponse.StartsWith("SUCCESS"))
            {

                var parser = PayPalPDTParser.Parse(strResponse);
                int paymentID = parser.Custom.Decrypt().Replace("tarts_", "").ConvertToInt32(0);
                var payment = Repo.GetById<Payment>(paymentID);
                view.Details = strResponse;
                if(payment == null)
                {
                    view.Success = false;
                    view.Message = "Sorry we could not match the payment to the payment reference, we will investigate this manually";
                    return View(view);
                }

                string debugInfo = strResponse;
                debugInfo += "<br/>" + cfg.PaypalUsername + ":" + parser.BusinessEmail;
                debugInfo += "<br/>" + payment.Amount.ToString() + ":" + parser.GrossTotal.ToString();
                //view.Details += "<br/>" + cfg.PaypalUsername + ":" +  parser.BusinessEmail;
                //view.Details += "<br/>" + payment.Amount.ToString() + ":" +  parser.GrossTotal.ToString();



                if (cfg.PaypalUsername != parser.BusinessEmail)
                {
                    payment.Booking.FailPayment(payment, "Receiver email looked to be invalid. " + debugInfo, PayPalPDTParser.ToString(parser));
                    Repo.Save(payment.Booking);

                    view.Success = false;
                    view.Message = "Payment looks to be invalid, we will investigate manually";
                    return View(view);
                }

                //if (Decimal.Round(payment.Amount,0).ToString() != Decimal.Round(parser.GrossTotal.ConvertToDecimal(0m),0).ToString())
                //{
                //    payment.Booking.FailPayment(payment, "Payment amount looked to be invalid. " + debugInfo, PayPalPDTParser.ToString(parser));
                //    Repo.Save(payment.Booking);
                    
                //    view.Success = false;
                //    view.Message = "Payment looks to be invalid, we will investigate manually";
                //    return View(view);
                //}
                
                
                if (parser.PaymentStatus.ToLower() == "completed")
                {
                    payment.Booking.MarkPaymentAsComplete(payment, PayPalPDTParser.ToString(parser));
                    Repo.Save(payment.Booking);
                    RedirectToAction("BookingComplete", "Booking", new {id = payment.Booking.ID});
                }
                else
                {
                    payment.Booking.MarkPaymentAsVerfied(payment, parser.PaymentStatus, PayPalPDTParser.ToString(parser));
                    Repo.Save(payment.Booking);

                    view.Success = true;
                    view.Message = "Thank you your payment has been verfied. We will email you as soon as it is processed and your booking is confirmed. ";
                    return View(view);
                }

                try
                {
                    if (FacebookWebContext.Current.IsAuthenticated())
                    {
                        var client = new FacebookWebClient();
                        dynamic me = client.Get("me");
                        var args = new Dictionary<string, object>();
                        args["message"] = string.Format("Has just bought their ticket for {0} at {1}!", payment.Booking.Event.Name, "http://www.appletartsfestival.co.uk");
                        client.Post("/me/feed", args);
                    }
                }
                catch 
                {
                    
                }
                
            }
                 
            view.Success = false;
            view.Message = "Payment has been sent but not yet verified";
            return View(view);
            
        }
    }
}
