using System;
using System.Text;
using System.Web;

namespace Tarts.Web.Infrastructure.Helpers
{
    public class PayPalPDTParser
    {
        public static string ToString(PayPalPDTParser parser)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("GrossTotal: {0} <br/>", parser.GrossTotal);
            sb.AppendFormat("Amount: {0} <br/>", parser.Amount);
            sb.AppendFormat("InvoiceNumber: {0} <br/>", parser.InvoiceNumber);
            sb.AppendFormat("PaymentStatus: {0} <br/>", parser.PaymentStatus);
            sb.AppendFormat("PayerFirstName: {0} <br/>", parser.PayerFirstName);
            sb.AppendFormat("PaymentFee: {0} <br/>", parser.PaymentFee);
            sb.AppendFormat("BusinessEmail: {0} <br/>", parser.BusinessEmail);
            sb.AppendFormat("TxToken: {0} <br/>", parser.TxToken);
            sb.AppendFormat("PayerLastName: {0} <br/>", parser.PayerLastName);
            sb.AppendFormat("ReceiverEmail: {0} <br/>", parser.ReceiverEmail);
            sb.AppendFormat("ItemName: {0} <br/>", parser.ItemName);
            sb.AppendFormat("Currency: {0} <br/>", parser.Currency);
            sb.AppendFormat("TransactionId: {0} <br/>", parser.TransactionId);
            sb.AppendFormat("SubscriberId: {0} <br/>", parser.SubscriberId);
            sb.AppendFormat("Custom: {0} <br/>", parser.Custom);
            sb.AppendFormat("Memo: {0} <br/>", parser.Memo);
            return sb.ToString();
        }

        public double GrossTotal { get; set; }
        public double Amount { get; set; }
        public int InvoiceNumber { get; set; }
        public string PaymentStatus { get; set; }
        public string PayerFirstName { get; set; }
        public double PaymentFee { get; set; }
        public string BusinessEmail { get; set; }
        public string PayerEmail { get; set; }
        public string TxToken { get; set; }
        public string PayerLastName { get; set; }
        public string ReceiverEmail { get; set; }
        public string ItemName { get; set; }
        public string Currency { get; set; }
        public string TransactionId { get; set; }
        public string SubscriberId { get; set; }
        public string Custom { get; set; }
        public string Memo { get; set; }

        public static PayPalPDTParser Parse(string postData)
        {
            String sKey, sValue;
            var ph = new PayPalPDTParser();

            try
            {
                //split response into string array using whitespace delimeter
                String[] StringArray = postData.Split('\n');

                // NOTE:
                /*
                * loop is set to start at 1 rather than 0 because first
                string in array will be single word SUCCESS or FAIL
                Only used to verify post data
                */

                // use split to split array we already have using "=" as delimiter
                int i;
                for (i = 1; i < StringArray.Length - 1; i++)
                {
                    String[] StringArray1 = StringArray[i].Split('=');

                    sKey = StringArray1[0];
                    sValue = HttpUtility.UrlDecode(StringArray1[1]);

                    // set string vars to hold variable names using a switch
                    switch (sKey)
                    {
                        case "memo":
                            ph.Memo = sValue;
                            break;

                        case "mc_gross":
                            ph.GrossTotal = Convert.ToDouble(sValue);
                            break;

                        case "amt":
                            ph.Amount = Convert.ToDouble(sValue);
                            break;

                        case "invoice":
                            ph.InvoiceNumber = Convert.ToInt32(sValue);
                            break;

                        case "payment_status":
                            ph.PaymentStatus = Convert.ToString(sValue);
                            break;

                        case "first_name":
                            ph.PayerFirstName = Convert.ToString(sValue);
                            break;

                        case "mc_fee":
                            ph.PaymentFee = Convert.ToDouble(sValue);
                            break;

                        case "business":
                            ph.BusinessEmail = Convert.ToString(sValue);
                            break;

                        case "payer_email":
                            ph.PayerEmail = Convert.ToString(sValue);
                            break;

                        case "Tx Token":
                            ph.TxToken = Convert.ToString(sValue);
                            break;

                        case "last_name":
                            ph.PayerLastName = Convert.ToString(sValue);
                            break;

                        case "receiver_email":
                            ph.ReceiverEmail = Convert.ToString(sValue);
                            break;

                        case "item_name":
                            ph.ItemName = Convert.ToString(sValue);
                            break;

                        case "mc_currency":
                            ph.Currency = Convert.ToString(sValue);
                            break;

                        case "txn_id":
                            ph.TransactionId = Convert.ToString(sValue);
                            break;

                        case "custom":
                            ph.Custom = Convert.ToString(sValue);
                            break;

                        case "subscr_id":
                            ph.SubscriberId = Convert.ToString(sValue);
                            break;
                    }
                }

                return ph;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}