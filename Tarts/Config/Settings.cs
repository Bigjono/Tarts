using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tarts.Base;

namespace Tarts.Config
{
    public class Settings : EntityBase
    {

        public virtual string AnalyticsTrackingCode { get; set; }

        public virtual string SmtpHost { get; set; }
        public virtual string SmtpUsername { get; set; }
        public virtual string SmtpPassword { get; set; }
        public virtual string ForceEmailsTo { get; set; }
        public virtual string EmailFromName { get; set; }
        public virtual string EmailFromAddress { get; set; }
            
        public virtual string PaypalUrl { get; set; }
        public virtual string PaypalPdtToken { get; set; }
        public virtual string PaypalUsername { get; set; }

        public virtual void UpdateSEOSettings(string analyticsTrackingCode)
        {
            AnalyticsTrackingCode = analyticsTrackingCode;
        }

        public virtual void UpdateEmailSettings(string smtpHost, string smtpUsername, string smtpPassword, string emailFromName, string emailFromAddress, string forceEmailsTo)
        {
            SmtpHost = smtpHost;
            SmtpUsername = smtpUsername;
            SmtpPassword = smtpPassword;
            ForceEmailsTo = forceEmailsTo;
            EmailFromName = emailFromName;
            EmailFromAddress = emailFromAddress;
        }

        public virtual void UpdatePaymentSettings(string paypalUrl, string paypalPdtToken, string paypalUsername)
        {
            PaypalUrl = paypalUrl;
            PaypalUsername = paypalUsername;
            PaypalPdtToken = paypalPdtToken;
        }


    }
}
