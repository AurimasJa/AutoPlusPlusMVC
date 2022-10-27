namespace AutoPlusPlusMVC.PaypalHelper
{
    public class PaypalAccessToken
    {
        public string scope { get; set; }
        public string access_token { get; set; }
        public string app_id { get; set; }
        public int expires_in { get; set; }
        public string nonce { get; set; }
    }
}
