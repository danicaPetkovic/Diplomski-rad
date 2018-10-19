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
    public class MapController : Controller
    {
        // GET: Map
        public ActionResult Index()
        {
            int userId = ((UserView)Session["User"]).Id_user;
            List<Place> places = PlaceDL.Data.UserPlaces(userId);
            List<PlaceView> placesView = new List<PlaceView>();

            foreach (Place place in places)
                placesView.Add(new PlaceView(place));

            System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var model = oSerializer.Serialize(placesView);

            ViewBag.places = model;

            return View();
        }

        public ActionResult Places()
        {
            int userId = ((UserView)Session["User"]).Id_user;
            List<Place> places = PlaceDL.Data.UserPlaces(userId);
            List<PlaceView> placesView = new List<PlaceView>();

            foreach (Place place in places)
                placesView.Add(new PlaceView(place));

            return Json(placesView);
        }

        public ActionResult ScorePlace(string title)
        {
            int userId = ((UserView)Session["User"]).Id_user;
            PlaceView place = new PlaceView(PlaceDL.Data.Place(userId, title));
            ((UserView)Session["User"]).Points += Int32.Parse(place.Points.ToString());

            return Json(place);
        }

        public ActionResult Comments(string id)
        {
            List<Comment> comments = PlaceDL.Data.GetComments(id);
            List<CommentView> commView = new List<CommentView>();

            foreach(Comment c in comments)
            {
                commView.Add(new CommentView(c));
            }

            System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var model = oSerializer.Serialize(commView);

            ViewBag.comments = model;
            ViewBag.place = id;
            return View();
        }

        public ActionResult AddComment()
        {
            List<CommentView> comm = new List<CommentView>();
            if (Request.Files.Count == 1)
            {
                HttpFileCollectionBase files = Request.Files;
                string fname;

                HttpPostedFileBase File = files[0];

                fname = File.FileName;
                string[] nameAndExtension = fname.Split('.');
                if (nameAndExtension[1].ToLower() == "jpg" || nameAndExtension[1].ToLower() == "png")
                {
                    fname = Path.Combine(Server.MapPath("~/Files"), fname);

                    File.SaveAs(fname);

                    Comment comment = new Comment();
                    comment.Id_place = Int32.Parse(Request.Form["place_id"]);
                    comment.Id_user = ((UserView)Session["User"]).Id_user;
                    comment.Type = "image";
                    comment.Comment1 = File.FileName;

                    PlaceDL.Data.AddComment(comment);

                    comm.Add(new CommentView(PlaceDL.Data.FindComment(fname)));
                }
            }

            string text = Request.Form["text"];
            if(text != null)
            {
                Comment comment = new Comment();
                comment.Id_place = Int32.Parse(Request.Form["place_id"]);
                comment.Id_user = ((UserView)Session["User"]).Id_user;
                comment.Type = "text";
                comment.Comment1 = text;

                PlaceDL.Data.AddComment(comment);

                comm.Add(new CommentView(PlaceDL.Data.FindComment(text)));
            }

            return Json(comm);
        }
    }
}