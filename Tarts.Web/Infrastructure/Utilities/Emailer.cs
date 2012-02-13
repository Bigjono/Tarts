using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using Bronson.Utils;
using Tarts.Config;

namespace Tarts.Web.Infrastructure.Utilities
{
    public class Emailer
    {

        #region Properties

        private readonly Settings SystemSettings;
        public string SmtpHost { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public string ForceEmailsTo { get; set; }
        public bool EmailsEnabled { get; set; }
        public string DefaultEmailFromName { get; set; }
        public string DefaultEmailFromAddress { get; set; }

        #endregion

        #region ctor

        public Emailer(Settings systemSettings)
        {
            SystemSettings = systemSettings;
            SmtpHost = SystemSettings.SmtpHost;
            SmtpUsername = SystemSettings.SmtpUsername;
            SmtpPassword = SystemSettings.SmtpPassword;
            ForceEmailsTo = SystemSettings.ForceEmailsTo;
            DefaultEmailFromAddress = SystemSettings.EmailFromAddress;
            DefaultEmailFromName = SystemSettings.EmailFromName;
            EmailsEnabled = true;
        }

        #endregion

        #region Public Methods

        public ReturnValue SendEmail(string toAddress, string toName, string subject, string messageBody,
                                     string fromName = "", string fromAddress = "")
        {
            var retVal = new ReturnValue() {Message = "Email Sent Successfully"};
            if (EmailsEnabled)
            {
                var message = GenerateMailMessage(toAddress, toName, subject, messageBody, fromName, fromAddress);
                SendMessage(retVal, message);
            }
            else retVal.SetFailMessage("Email sending functionality is currently disabled");
            return retVal;
        }

        #endregion

        #region Private Methods

        private void SendMessage(ReturnValue retVal, MailMessage message)
        {
            try
            {
                var client = new SmtpClient(SmtpHost);
                if ((!string.IsNullOrWhiteSpace(SmtpUsername)) && (!string.IsNullOrWhiteSpace(SmtpPassword)))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(SmtpUsername, SmtpPassword);
                }
                client.Send(message);
            }
            catch (Exception ex)
            {
                retVal.SetFailMessage("Failed to send email: " + ex.Message);
            }
        }

        private MailMessage GenerateMailMessage(string toAddress, string toName, string subject, string messageBody,
                                                string fromName, string fromAddress)
        {
            var message = new MailMessage();
            fromName = (!string.IsNullOrEmpty(fromName)) ? fromName : DefaultEmailFromName;
            fromAddress = (!string.IsNullOrEmpty(fromAddress)) ? fromAddress : DefaultEmailFromAddress;
            if (!string.IsNullOrEmpty(ForceEmailsTo))
            {
                toAddress = ForceEmailsTo;
            }

            message.From = new MailAddress(fromAddress, fromName);
            message.To.Add(new MailAddress(toAddress, toName));
            message.Subject = subject;
            message.Body = messageBody;
            message.IsBodyHtml = true;

            return message;
        }

        #endregion
    }
}