using AutoPlusPlusMVC.Data;
using AutoPlusPlusMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Reflection;
namespace AutoPlusPlusMVC.Controllers
{
    public class InspectorController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        public InspectorController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;

            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult WorkShiftView()
        {
            MergeInspTimes mergeInspTimes = new MergeInspTimes();
           // List<Inspector_times> inspector_times = new List<Inspector_times>();

            List<Inspector_times> getRequestedInspectionDates = _db.Inspector_times
                .Where(t => t.fk_Userid_User == Session.LoggedInUser.id_User)
                .Include(i => i.fk_User)
                .ToList();

        /*    foreach (var item in inspector_times)
            {
                Debug.WriteLine("AAAA");
                Debug.WriteLine(item.interval_end);
                Debug.WriteLine(item.fk_User.name);
            }*/

            List<Inspection> getSavedInspectionDates = _db.Inspection
                .Include(x => x.fk_User)
                .Include(x => x.fk_Inspector)
                .Include(x => x.fk_Listing)
                .Where(x => x.fk_Inspector.id_User == Session.LoggedInUser.id_User)
                .ToList();

            mergeInspTimes.inspections = getSavedInspectionDates;
            mergeInspTimes.inspector_Times = getRequestedInspectionDates;


            return View(mergeInspTimes);
        }
        [HttpPost]
        public IActionResult create(MergeInspTimes model)
        {
            if (model.inspector.interval_start > model.inspector.interval_end || String.IsNullOrEmpty(model.inspector.inspection_price.ToString()) || model.inspector.inspection_price < 1)
            {
                TempData["ErrorMessage"] = "Klaidingai įvesti duomenys";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            Inspector_times inspector_Times = new Inspector_times();
            inspector_Times.interval_start = model.inspector.interval_start;
            inspector_Times.interval_end = model.inspector.interval_end;
            inspector_Times.inspection_price = model.inspector.inspection_price;
            inspector_Times.fk_Userid_User = Session.LoggedInUser.id_User;

            // DateTime s2 = enddate;//
            // float s3 = price;//
            // inspector_Times.interval_start = s1;
            // inspector_Times.interval_end = s2;
            // inspector_Times.inspection_price = s3;\

            _db.Inspector_times.Add(inspector_Times);
            _db.SaveChanges();

            return Redirect(Request.Headers["Referer"].ToString());
        }
       
    }
}
