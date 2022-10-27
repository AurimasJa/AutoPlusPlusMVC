//using PayPal.Api;
using System;
//using PayPal;

namespace AutoPlusPlusMVC.PaypalHelper
{
    public class PaypalPaymentCreatedResponse
    {
        public string id { get; set; }
        public string intent { get; set; }
        public string state { get; set; }
        public Payer payer { get; set; }
        public Transaction[] transactions { get; set; }
        public DateTime create_time { get; set; }

    }
    public class Payer
    {
        public string payment_method { get; set; }
    }
    public class Amount
    {
        public string total { get; set; }
        public string currency { get; set; }
    }
    public class Transaction
    {
        public double amount { get; set; }
        public Link link { get; set; }

        public string businessEmail = "appbusiness@business.example.com";

        public Transaction(double amount)
        {
            this.amount = amount;
            this.link = new Link("https://www.sandbox.paypal.com/" +
                $"cgi-bin/webscr?cmd=_xclick&amount={amount}&" +
                $"business={businessEmail}&" +
                $"item_name=Saskaitos_pildymas&return=https://localhost:7001/User/TransactionReportView");
        }
    }
    public class Link
    {
        public string href { get; set; }
        public string rel { get;set; }
        public string method { get; set; }

        public Link(string href)
        {
            this.href = href;
        }
    }
}
