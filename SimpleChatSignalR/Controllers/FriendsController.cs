using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Baza.DataLayer;
using Baza;
using SimpleChatSignalR.Models;

namespace SimpleChatSignalR.Controllers
{
    public class FriendsController : Controller
    {
        // GET: Friends
        public ActionResult Index()
        {
            ViewBag.recvId = "";
            ViewBag.recvImage = "";
            ViewBag.Id = ((UserView)Session["User"]).Id_user;
            return View();
        }

        public ActionResult Friends()
        {
            UserView user = (UserView)Session["User"];
            List<User> users = UserDL.Data.UsersFriends(user.Id_user);
            List<UserView> friends = new List<UserView>();
            foreach (User u in users)
                friends.Add(new UserView(u));

            return Json(friends);
        }

        public ActionResult Autocomplete(string name)
        {
            string[] names = name.Split(' ');
            UserView user = new UserView(UserDL.Data.FindUserByName(names[0], names[1]));
            return Json(user);
        }

        public ActionResult DeleteFriend(int id)
        {
            int users_id = ((UserView)Session["User"]).Id_user;
            UserDL.Data.DeleteFriend(users_id, id);
            return Json(true);
        }

        public ActionResult Chat(string id, string image)
        {
            if(image == "")
            {
                string imageUser = "../Files/" + UserDL.Data.ReturnUserImage(Int32.Parse(id));
                Session["recvImage"] = imageUser;
            }
            else
                Session["recvImage"] = image;

            Session["id"] = id;

            return View();
        }

        public ActionResult AllMessages(string userId, string recvId)
        {
            if (recvId == null)
                return Json(false);
            List<MessageView> messagesView = new List<MessageView>();
            List<Message> messages = UserDL.Data.AllMessages(Int32.Parse(userId), Int32.Parse(recvId));

            foreach(Message m in messages)
            {
                messagesView.Add(new MessageView(m));
            }

            //messagesView.OrderBy(d => d.date_time);
            return Json(messagesView);
        }

        public ActionResult FindUser(string name)
        {
            string[] names = name.Split(' ');

            UserView user = new UserView(UserDL.Data.FindUserByName(names[0], names[1]));

            return Json(user);
        }

        public ActionResult AddFriend(string id)
        {
            UserDL.Data.AddFriend(((UserView)Session["User"]).Id_user, Int32.Parse(id));

            return Json(true);
        }
    }
}