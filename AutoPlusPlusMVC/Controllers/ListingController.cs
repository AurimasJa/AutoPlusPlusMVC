using AutoPlusPlusMVC.Data;
using AutoPlusPlusMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace AutoPlusPlusMVC.Controllers
{
    public class ListingController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public ListingController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;

            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RememberListing(int id)
        {
            if(Session.LoggedInUser is null)
            {
                return View(ListingView(id));
            }

            listing_remembrance favorite = new listing_remembrance();
            favorite.date = DateTime.Now;
            favorite.User = _db.User.Where(x => x.email.Equals(Session.LoggedInUser.email)).FirstOrDefault();
            favorite.Listing = _db.Listing.Where(x => x.id_Listing == id).FirstOrDefault();
            _db.Listing_remembrance.Add(favorite);
            _db.SaveChanges();

            //_db.Listing.Add(listing);
            //_db.SaveChanges();
            TempData["SuccessMessage"] = "Skelbimas įsimintas";

            //return View(ListingView(id));
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult ListingView(int id)
        {
            MergePhotosListings mergePhotosListings = new MergePhotosListings();
            Listing listing = _db.Listing.Where(x => x.id_Listing == id).FirstOrDefault();
            var photos = _db.Photo.Where(x => x.Listing.id_Listing == id).ToList();

            mergePhotosListings.photos = photos;


            listing.remembrances = _db.Listing_remembrance.Include(x => x.User).Where(f => f.Listing.id_Listing == id).ToList();
            mergePhotosListings.listing = listing;
            return View(mergePhotosListings);
        }
        public IActionResult ListingListView()
        {
            


            List<Listing> listings = _db.Listing
                .OrderByDescending(x => x.listing_creation_date)
                .Include(x => x.photos)
                .ToList();

            //return View(_db.Listing.ToList());
            return View(listings);
        }

        public IActionResult EditListingView(int id)
        {
            return View(_db.Listing.Where(x => x.id_Listing == id).FirstOrDefault());
        }

        [HttpPost]
        public IActionResult EditListing(int id)
        {
            Listing listing = (from x in _db.Listing
                                      where x.id_Listing == id
                                      select x).FirstOrDefault();

            if (listing != null)
            {
                var nvc = Request.Form;
                listing.listing_creation_date = DateTime.Now;
                listing.fuel_type = Request.Form["fuel_type"];
                listing.mileage = double.Parse(nvc["mileage"]);
                listing.vin = nvc["vin"];
                listing.engine_displacement = nvc["engine_displacement"];
                listing.model = nvc["model"];
                listing.make = Request.Form["make"];
                listing.number_of_doors = int.Parse(nvc["number_of_doors"]);
                listing.gearbox = Request.Form["gearbox"];
                listing.body_type = Request.Form["body_type"];
                listing.year = DateTime.Parse(nvc["year"]);
                listing.price = double.Parse(nvc["price"]);
                listing.drive_wheels = int.Parse(Request.Form["drive_wheels"]);
                listing.power = nvc["power"];
                listing.color = Request.Form["color"];
                listing.number_of_seats = int.Parse(nvc["number_of_seats"]);
                listing.steering_wheel_position = Request.Form["steering_wheel_position"];
                listing.first_registration_country = nvc["first_registration_country"];
                listing.co2_emissions = double.Parse(nvc["co2_emissions"]);
                listing.city = nvc["city"];
                listing.country = nvc["country"];
                listing.phone_number = nvc["phone_number"];
                listing.fk_Userid_User = Session.LoggedInUser.id_User;
                _db.Listing.Update(listing);
                _db.SaveChanges();
            }

            return Redirect("/Listing/ListingListView");
        }

        public IActionResult DeleteListingView(int id)
        {
            Listing listing = _db.Listing.Find(id);   
            Photo photo = _db.Photo.Where(x => x.Listing.id_Listing == id).FirstOrDefault();
            List<listing_remembrance> listing_Remembrance = _db.Listing_remembrance.Where(x => x.Listing.id_Listing == id).ToList();
            _db.Photo.Remove(photo);
            _db.Listing_remembrance.RemoveRange(listing_Remembrance);
            _db.Listing.Remove(listing);        
            _db.SaveChanges();
            return Redirect("/Listing/ListingListView");
        }
        public IActionResult CreateListingView()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateListing()
        {
            var nvc = Request.Form;
            Listing listing = new Listing();
            listing.listing_creation_date = DateTime.Now;
            //listing.fuel_type = nvc["fuel_type"];
            listing.fuel_type = Request.Form["fuel_type"];
            listing.mileage = double.Parse(nvc["mileage"]);
            listing.vin = nvc["vin"];
            listing.engine_displacement = nvc["engine_displacement"];
            listing.model = nvc["model"];
            listing.make = Request.Form["make"];
            listing.number_of_doors = int.Parse(nvc["number_of_doors"]);
            listing.gearbox = Request.Form["gearbox"];
            listing.body_type = Request.Form["body_type"];
            listing.year = DateTime.Parse(nvc["year"]);
            listing.price = double.Parse(nvc["price"]);
            listing.drive_wheels = int.Parse(Request.Form["drive_wheels"]);
            listing.power = nvc["power"];
            listing.color = Request.Form["color"];
            listing.number_of_seats = int.Parse(nvc["number_of_seats"]);
            listing.steering_wheel_position = Request.Form["steering_wheel_position"];
            listing.first_registration_country = nvc["first_registration_country"];
            listing.co2_emissions = double.Parse(nvc["co2_emissions"]);
            listing.city = nvc["city"];
            listing.country = nvc["country"];
            listing.phone_number = nvc["phone_number"];
            listing.fk_Userid_User = Session.LoggedInUser.id_User;

            _db.Listing.Add(listing);
            _db.SaveChanges();           

            foreach (var item in Request.Form.Files)
            {
                string path = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
                using (var stream = new FileStream(@$"{path}\wwwroot\css\" + item.FileName, FileMode.Create))
                {
                    item.CopyTo(stream);
                    Photo photo = new Photo();
                    photo.path = item.FileName;
                    photo.Listing = listing;
                    _db.Photo.Add(photo);
                }
            }

            _db.SaveChanges();

            int id = listing.id_Listing;

            return Redirect("/Listing/ListingView/" + id);
        }
        public double calculatePrice(List<Listing> relevantListings)
        {
            double finPrice = 0;
            double suggestedPrice = 0;
            foreach (var item in relevantListings)
            {
                suggestedPrice += item.price;

            }

            finPrice = suggestedPrice / relevantListings.Count();
            return finPrice;
        }
        [HttpPost]
        public IActionResult PredictPrice()
        {


            List<Listing> listings = _db.Listing
                .Include(x => x.photos)
                .ToList();

            List<Listing> relevantListings = new List<Listing>();

            DateTime inputDate = Convert.ToDateTime(Request.Form["year"]);

            foreach (var item in listings)
            {
                if (item.make == Request.Form["make"] && item.model == Request.Form["model"] && inputDate.Year - 2 < item.year.Year && item.year.Year < inputDate.Year + 2)
                {
                    relevantListings.Add(item);
                }
            }

            if (relevantListings.Count > 0)
            {
                double finPrice = calculatePrice(relevantListings);
                TempData["Message"] = "Rekomenduojama skelbimo kaina: " + finPrice + "€";
            }
            else
            {
                TempData["Message"] = "Nerasta panašių skelbimų, kainos prognozė negalima";
            }

            return Redirect("/Listing/CreateListingView");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}