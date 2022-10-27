using AutoPlusPlusMVC.Data;
using AutoPlusPlusMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Reflection;

using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;

namespace AutoPlusPlusMVC.Controllers
{
    public class ForumController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public ForumController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public static void SendMailToTopicOwner(string toEmail)
        {

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("AutoPlusPlusReal@gmail.com", "autoplusplusmvc"),
                EnableSsl = true,
            };

            smtpClient.Send("AutoPlusPlusReal@gmail.com", toEmail, "AutoPlusPlus Forumo atsakymas", "Sveiki, jusu forumo tema sulauke atsakymo");
        }

  

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ForumListView()
        {
            List<Forum_topic> getAllTopics = _db.Forum_topic
                .Include(i => i.fk_User)
                .ToList();

            return View(getAllTopics);
        }

        [HttpPost]
        public IActionResult ForumAnswerEdit(int id)
        {
            Forum_answer answer = _db.Forum_answer.Include(x => x.fk_Forum_topic).Where(x => x.id_Forum_answer == id).FirstOrDefault();

            
            var nvc = Request.Form;
            answer.text = Request.Form["text"];

            if (answer != null && answer.text != null && answer.text.Length > 1)
            {
                _db.Forum_answer.Update(answer);
                _db.SaveChanges();
                return Redirect($"/Forum/ForumThemeView/{answer.fk_Forum_topic.id_Forum_topic}");
            }
            else
            {
                TempData["ErrorMessage"] = "Įvyko klaida";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            
        }
        public IActionResult ThemeAnswerEditView(int id)
        {
            Forum_answer answer = _db.Forum_answer.Where(x => x.id_Forum_answer == id).FirstOrDefault();
            return View(answer);
        }

        [HttpPost]
        public IActionResult AcceptAnswer(MergeRemembForumTop merge)
        {
            if(merge.forum_Answer.text != null && merge.forum_Answer.text.Length > 0)
            {
                Forum_answer answer = new Forum_answer();
                answer.text = merge.forum_Answer.text;
                answer.fk_User = _db.User.Where(x => x.id_User == Session.LoggedInUser.id_User).FirstOrDefault();

                merge.forum_Topic = Session.Lmao;

                answer.fk_Forum_topic = _db.Forum_topic.Where(x => x.id_Forum_topic == merge.forum_Topic.id_Forum_topic).FirstOrDefault();

                _db.Forum_answer.Add(answer);
                _db.SaveChanges();

                SendMailToTopicOwner(merge.forum_Topic.fk_User.email);
            }
            else
            {
                TempData["ErrorMessage"] = "Ivyko klaida";
            }


            return Redirect(Request.Headers["Referer"].ToString());
        }


        public IActionResult ForumThemeView(int id)
        {
            var topic = _db.Forum_topic.Where(x => x.id_Forum_topic == id).Include(i => i.fk_User).FirstOrDefault();
            topic.answers = _db.Forum_answer.Include(x => x.fk_User).Where(x => x.fk_Forum_topic.id_Forum_topic == id).ToList();
            MergeRemembForumTop mergeRemembForumTop = new MergeRemembForumTop();
            mergeRemembForumTop.forum_Topic = topic;

            if (Session.LoggedInUser != null)
            {
                Forum_topic_remembrance ifRemembered = _db.Forum_topic_remembrance.Where(x => x.fk_Forum_topic.id_Forum_topic == id
                && x.fk_User.id_User == Session.LoggedInUser.id_User).FirstOrDefault();
                mergeRemembForumTop.forum_Topic_Remembrances = ifRemembered;
            }
            return View(mergeRemembForumTop);
        }
        public IActionResult BumpTopic(int id)
        {
            if(Session.LoggedInUser != null)
            {
                User user = _db.User.Where(x => x.id_User == Session.LoggedInUser.id_User).FirstOrDefault();
                if (user.balance >= 4.99)
                {


                    Forum_topic bumpedTopic = _db.Forum_topic.Where(x => x.id_Forum_topic == id).FirstOrDefault();
                    if(bumpedTopic is null)
                    {
                        TempData["ErrorMessage"] = "Ivyko klaida";
                    }
                    else
                    {
                        user.balance -= 4.99;
                        Session.LoggedInUser.balance -= 4.99;
                        _db.Update(user);
                        bumpedTopic.bumped = 1;
                        _db.Update(bumpedTopic);
                        _db.SaveChanges();

                        
                    }

                    TempData["SuccessMessage"] = "Tema sėkmingai iškelta už $4.99";

                }
                else
                {
                    TempData["ErrorMessage"] = "Nepakankamas likutis";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Ivyko klaida";
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult ForumThemeEditView(int id)
        {
            var topic = _db.Forum_topic.Where(x => x.id_Forum_topic == id).Include(i => i.fk_User).FirstOrDefault();

            Debug.WriteLine(topic);
            return View(topic);
        }

        [HttpPost]
        public IActionResult ForumThemeEdit(int id)
        {
            Forum_topic forum = (from x in _db.Forum_topic
                               where (x.id_Forum_topic == id)
                               select x).FirstOrDefault();

            var nvc = Request.Form;
            forum.name = Request.Form["name"];
            forum.text = Request.Form["text"];

            if (forum != null && forum.name != null && forum.name.Length > 1 && forum.text != null && forum.text.Length > 1 )
            {
                _db.Forum_topic.Update(forum);
                _db.SaveChanges();
                return Redirect($"/Forum/ForumThemeView/{forum.id_Forum_topic}");
            }
            else
            {
                TempData["ErrorMessage"] = "Įvyko klaida";
                return Redirect(Request.Headers["Referer"].ToString());
            }
        }

        public IActionResult ForumThemeDeleteView (int id)
        {
            Forum_topic forum_topic = _db.Forum_topic.Where(x => x.id_Forum_topic == id).FirstOrDefault();
            List<Forum_topic_remembrance> forum_topic_remembrance = _db.Forum_topic_remembrance.Where(x => x.fk_Forum_topic.id_Forum_topic == id).ToList();
            _db.Forum_topic_remembrance.RemoveRange(forum_topic_remembrance);
            _db.Remove(forum_topic);

            _db.SaveChanges();
            return Redirect("/Forum/ForumListView");
        }

        public IActionResult RememberTopic(int id)
        {
            if (Session.LoggedInUser is null)
            {
                return View(ForumThemeView(id));
            }


            Forum_topic_remembrance favorite = new Forum_topic_remembrance();
            favorite.date = DateTime.Now;
            favorite.fk_User = _db.User.Where(x => x.email.Equals(Session.LoggedInUser.email)).FirstOrDefault();
            favorite.fk_Forum_topic = _db.Forum_topic.Where(x => x.id_Forum_topic == id).FirstOrDefault();
            _db.Forum_topic_remembrance.Add(favorite);
            _db.SaveChanges();

            //_db.Listing.Add(listing);
            //_db.SaveChanges();
            TempData["SuccessMessage"] = "Tema įsiminta";

            //return View(ListingView(id));
            return Redirect(Request.Headers["Referer"].ToString());


        }
        public IActionResult ThemeCreateView()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForumThemeCreate()
        {
            var nvc = Request.Form;
            Forum_topic topic = new Forum_topic();
            topic.fk_User = _db.User.Where(x => x.id_User == Session.LoggedInUser.id_User).FirstOrDefault();
            topic.name = nvc["name"];
            topic.creation_date = DateTime.Now;
            topic.text = nvc["text"];
            topic.bumped = 0;

            _db.Forum_topic.Add(topic);
            _db.SaveChanges();

            int id = topic.id_Forum_topic;

            return Redirect("/Forum/ForumThemeView/" + id);
        }
    }
}