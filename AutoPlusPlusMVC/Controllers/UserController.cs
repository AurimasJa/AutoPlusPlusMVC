using AutoPlusPlusMVC.Data;
using AutoPlusPlusMVC.Models;
using AutoPlusPlusMVC.PaypalHelper;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using PayPal;
using System.Diagnostics;

namespace AutoPlusPlusMVC.Controllers
{

    public class UserController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public UserController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string a, string b)
        {
            var nvc = Request.Form;

            string email = nvc["email"];
            string password = nvc["password"];

            if (_db.User.Where(x => x.email.Equals(email)).FirstOrDefault() == null ||
                !_db.User.Where(x => x.email.Equals(email)).FirstOrDefault().password.Equals(password))
            {
                TempData["ErrorMessage"] = "Klaidingai ivesti duomenys";
                return View();
            }

            Session.LoggedInUser = _db.User.Where(x => x.email.Equals(email)).FirstOrDefault();
            return Redirect("/");
        }

        public IActionResult AddBalanceView()
        {
            return View(Session.LoggedInUser);
        }

        public IActionResult ProfileView()
        {
            return View(Session.LoggedInUser);
        }

        [HttpPost]
        public IActionResult SendTransactionData()
        {
            var nvc = Request.Form;
            double sum = double.Parse(nvc["balance"]);
            Debug.WriteLine(sum);

            Transaction transaction = new Transaction(sum);
            Session.recentTransaction = transaction;
            return Redirect(transaction.link.href);
            //return View(Session.LoggedInUser);
        }

        public IActionResult TransactionReportView()
        {
            if (Session.recentTransaction != null && Session.recentTransaction.amount >= 5 && Session.LoggedInUser != null)
            {
                User user = _db.User.Where(x => x.id_User == Session.LoggedInUser.id_User).FirstOrDefault();
                user.balance += Session.recentTransaction.amount;

                _db.Update(user);
                Balance_payments payment = new Balance_payments();
                payment.User = user;
                payment.sum = Session.recentTransaction.amount;
                _db.Balance_payments.Add(payment);
                _db.SaveChanges();

                Session.LoggedInUser.balance += Session.recentTransaction.amount;
                return View();
            }
            else
            {
                Session.recentTransaction = null;
                TempData["ErrorMessage"] = "Ivyko klaida";
                return Redirect("/User/AddBalanceView");
            }
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterUser()
        {
            var nvc = Request.Form;

            string name = nvc["name"];
            string surname = nvc["surname"];
            string password = nvc["password"];
            string email = nvc["email"];
            string city = nvc["city"];
            DateTime date_of_birth = DateTime.Parse(nvc["date_of_birth"]);
            Global.type type = Global.type.user;
            double balance = 0;
            string phone_number = nvc["phone_number"];
            Global.abilities abilities = Global.abilities.allow_all;

            return Redirect("/");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class PaypalConfiguration
    {
        public static string ClientId;
        public static string ClientSecret;

        static PaypalConfiguration()
        {
            var config = GetConfig();
            ClientId = "AZkFfuMTlrUutw71H6q9RaucfWtw7ip1pwa_U-PziE1tqXbMRtBUiBTxFrHmmvehlmL3ESrxBwRRp5Vb";
            ClientSecret = "EM2REGTy2rbGqzm8CCv0iE7zNbMNe_sllcbrrZmRCcPFEvHrAsstOYGP4zm-SclKuEB_8ugmd87BCdKo";
        }

        public static Dictionary<string, string> GetConfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }

        //Create access token
        private static string GetAccessToken()
        {
            string acessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
            return acessToken;
        }

        //Apicontext obj
        public static APIContext GetAPIContext()
        {
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }

    }
}