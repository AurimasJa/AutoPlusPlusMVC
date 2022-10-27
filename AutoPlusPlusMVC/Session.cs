using AutoPlusPlusMVC.Models;
using AutoPlusPlusMVC;
using AutoPlusPlusMVC.PaypalHelper;

namespace AutoPlusPlusMVC
{
    public static class Session
    {
        public static User LoggedInUser = null;

        public static Transaction recentTransaction = null;

        public static Forum_topic Lmao = null;
    }
}
