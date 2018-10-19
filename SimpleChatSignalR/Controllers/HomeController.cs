using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Baza;
using Baza.DataLayer;
using SimpleChatSignalR.Models;

namespace SimpleChatSignalR.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sign_in(string username, string password)
        {
            User user = UserDL.Data.FindUser(username, password);
            bool redirect;
            if (PlaceDL.Data.UserScores(user.Id_user))
            {
                redirect = true;
            }
            else
                redirect = false;


            if (user != null)
            {
                int score = UserDL.Data.GetScore(user.Id_user);
                Session["User"] = new UserView(user);
                ((UserView)Session["User"]).Points = score;

                return Json(redirect);
            }
            else
                return Json(false);
        }

        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(User user)
        {
            if (Request.Files.Count == 1)
            {
                HttpFileCollectionBase files = Request.Files;
                string fname;

                HttpPostedFileBase file = files[0];

                fname = file.FileName;
                string[] nameAndExtension = fname.Split('.');
                if (nameAndExtension[1].ToLower() == "jpg" || nameAndExtension[1].ToLower() == "png")
                {
                    fname = Path.Combine(Server.MapPath("~/Files"), fname);

                    file.SaveAs(fname);
                    user.Image = file.FileName;
                    UserDL.Data.CreateUser(user);
                    user = UserDL.Data.FindUser(user.Username, user.Password);
                    int score = UserDL.Data.GetScore(user.Id_user);
                    Session["User"] = new UserView(user);
                    ((UserView)Session["User"]).Points = score;
                }
            }
            else
            {
                return Json(false);
            }
            return Json(true);
        }

        public ActionResult Main_page()
        {
            return View();
        }

        public ActionResult Start(bool landmarks, bool clubs, bool museums, bool interesting)               //deo za mapu
        {
            PlaceDL.Data.ConnectPlacesWithUser(landmarks, museums, clubs, interesting, ((UserView)Session["User"]).Id_user);
            return Json(true);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            Session["User"] = null;
            return RedirectToAction("Index");
        }

        public JsonResult GetNotifications()
        {
            var notificationRegisterTime = Session["LastUpdated"] != null ? Convert.ToDateTime(Session["LastUpdated"]) : DateTime.Now;
            NotificationComponent nc = new NotificationComponent();
            var list = nc.GetMessages(notificationRegisterTime);

            Session["LastUpdate"] = DateTime.Now;

            return Json(list);
        }

        public JsonResult GetImage(string userId)
        {
            return Json(UserDL.Data.ReturnUserImage(Int32.Parse(userId)));
        }

        public JsonResult GetFriend(string userId)
        {
            return Json(new UserView(UserDL.Data.GetUser(Int32.Parse(userId))));
        }

        public ActionResult Scores()
        {
            return View();
        }

        public JsonResult WorldScore()
        {
            return Json(WorldScoreForPlaces());
        }

        public JsonResult FriendsScore()
        {
            return Json(FriendScoreForPlaces());
        }

        private List<UserView> WorldScoreForPlaces()
        {
            using (AttractionsDBContext db = new AttractionsDBContext())
            {
                List<User> users = db.Users.ToList();
                List<UserView> userViews = new List<UserView>();

                foreach(User u in users)
                {
                    List<int?> points = db.Scores.Where(d => d.Id_user == u.Id_user && d.visited == 1).Select(d => d.Place.Points).ToList();
                    int score = 0;
                    foreach(int? point in points)
                    {
                        score += Int32.Parse(point.ToString());
                    }
                    UserView userView = new UserView(u);
                    userView.Points = score;
                    userViews.Add(userView);
                }

                return userViews;
            }
        }

        private List<UserView> FriendScoreForPlaces()
        {
            List<UserView> users = WorldScoreForPlaces();

            int id = ((UserView)Session["User"]).Id_user;
            List<User> usersFriends = UserDL.Data.UsersFriends(id);

            foreach(User u in usersFriends)
            {
                users.Remove(users.Where(d => d.Id_user == u.Id_user).First());
            }

            users.Remove(users.Where(d => d.Id_user == id).FirstOrDefault());

            return users;
        }
    }
}

       