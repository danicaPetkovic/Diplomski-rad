using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.DataLayer
{
    public class PlaceDL
    {
        private static PlaceDL data;
        AttractionsDBContext db;

        private PlaceDL() { }

        public static PlaceDL Data
        {
            get
            {
                if (data == null)
                {
                    data = new PlaceDL();

                }
                return data;
            }
        }

        public void ConnectPlacesWithUser(bool landmarks, bool museus, bool clubs, bool other, int userId)
        {
            using (db = new AttractionsDBContext())
            {
                List<Place> places = new List<Place>();
                places = db.Places.ToList();

                foreach(Place place in places)
                {
                    if(place.Category == "landmark" && landmarks)
                    {
                        Score score = new Score();
                        score.Id_place = place.Id_place;
                        score.Id_user = userId;

                        db.Scores.Add(score);
                    }
                    else if (place.Category == "museum" && museus)
                    {
                        Score score = new Score();
                        score.Id_place = place.Id_place;
                        score.Id_user = userId;

                        db.Scores.Add(score);
                    }
                    else if (place.Category == "restaurant" && clubs)
                    {
                        Score score = new Score();
                        score.Id_place = place.Id_place;
                        score.Id_user = userId;

                        db.Scores.Add(score);
                    }
                    else if (place.Category == "other" && other)
                    {
                        Score score = new Score();
                        score.Id_place = place.Id_place;
                        score.Id_user = userId;

                        db.Scores.Add(score);
                    }
                }
                db.SaveChanges();
            }
        }

        public void AddComment(Comment comment)
        {
            using (AttractionsDBContext db = new AttractionsDBContext())
            {
                db.Comments.Add(comment);
                db.SaveChanges();
            }
        }

        public Comment FindComment(string comment)
        {
            using (AttractionsDBContext db = new AttractionsDBContext())
            {
                return db.Comments.Where(d => d.Comment1 == comment).FirstOrDefault();
            }
        }

        public List<Comment> GetComments(string Id_place)
        {
            using (AttractionsDBContext db = new AttractionsDBContext())
            {
                int id = Int32.Parse(Id_place);
                return db.Comments.Where(d => d.Id_place == id).ToList();
            }
        }

        public List<Place> UserPlaces(int userId)
        {
            using (db = new AttractionsDBContext())
            {
                List<Score> scores = (from s in db.Scores where s.Id_user == userId select s).ToList();
                List<Place> places = (from s in db.Places select s).ToList();

                List<Place> userPlaces = new List<Place>();

                foreach(Score score in scores)
                {
                    foreach(Place place in places)
                    {
                        if (score.Id_place == place.Id_place)
                            userPlaces.Add(place);
                    }
                }

                return userPlaces;
            }
        }

        public bool UserScores(int userId)
        {
            using (db = new AttractionsDBContext())
            {
                Score score = (from s in db.Scores where s.Id_user == userId select s).FirstOrDefault();
                if (score == null)
                    return false;
                return true;
            }
        }

        public int? ScorePlace(int userId, string title)
        {
            using (AttractionsDBContext db = new AttractionsDBContext())
            {
                Place place = (from p in db.Places where p.Name == title select p).FirstOrDefault();
                db.Scores.Where(d => d.Id_user == userId && d.Id_place == place.Id_place).First().visited = 1;
                db.SaveChanges();

                return place.Points;
            }
        }

        public Place Place(int userId, string title)
        {
            using (AttractionsDBContext db = new AttractionsDBContext())
            {
                Place place = (from p in db.Places where p.Name == title select p).FirstOrDefault();
                db.Scores.Where(d => d.Id_user == userId && d.Id_place == place.Id_place).First().visited = 1;
                db.SaveChanges();

                return place;
            }
        }
    }
}
