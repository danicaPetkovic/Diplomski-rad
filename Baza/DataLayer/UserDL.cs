using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.DataLayer
{
    public class UserDL
    {
        private static UserDL data;
        AttractionsDBContext db;

        private UserDL() { }

        public static UserDL Data
        {
            get
            {
                if (data == null)
                {
                    data = new UserDL();
                }
                return data;
            }
        }

        public void CreateUser(User user)
        {
            using (db = new AttractionsDBContext())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
        }

        public User FindUser(string username, string password)
        {
            using (db = new AttractionsDBContext())
            {
                return (from u in db.Users where u.Username == username && u.Password == password select u).FirstOrDefault();
            }
        }

        public User FindUserByName(string name, string surname)
        {
            using (db = new AttractionsDBContext())
            {
                return (from u in db.Users where u.Name == name && u.Surname == surname select u).FirstOrDefault();
            }
        }

        public void DeleteFriend(int users_id, int friends_id)
        {
            using (db = new AttractionsDBContext())
            {
                db.Friends.Remove(db.Friends.Where(data => data.Id_user == users_id && data.Id_user_friend == friends_id).FirstOrDefault());
                db.SaveChanges();
            }
        }

        public List<User> UsersFriends(int id)
        {
            using (db = new AttractionsDBContext())
            {
                IList<User> friends = new List<User>();
                List<User> realFriends = new List<User>();
                IList<int?> users = new List<int?>();
      
                users = (from f in db.Friends where f.Id_user == id select f.Id_user_friend).ToList();

                friends = db.Users.ToList();

                foreach(User u in friends)
                {
                    foreach(int i in users)
                    {
                        if (u.Id_user == i)
                            realFriends.Add(u);
                    }
                }
                return realFriends;
            }
        }

        public void AddMessage(Message message)
        {
            using (db = new AttractionsDBContext())
            {
                db.Messages.Add(message);
                db.SaveChanges();
            }
        }

        public List<Message> AllMessages(int userId, int user_Id)
        {
            using (db = new AttractionsDBContext())
            {
                //List<Message> messages = (from m in db.Messages where m.Id_send_user == userId && m.Id_recv_user == user_Id select m).ToList();
                //messages.AddRange((from m in db.Messages where m.Id_send_user == user_Id && m.Id_recv_user == userId select m).ToList());
                //messages.OrderBy(m => m.date_time);

                List<Message> messages = db.Messages.ToList();
                List<Message> messReturn = new List<Message>();

                foreach(Message m in messages)
                {
                    if(m.Id_send_user == userId && m.Id_recv_user == user_Id)
                        messReturn.Add(m);
                    else if(m.Id_send_user == user_Id && m.Id_recv_user == userId)
                        messReturn.Add(m);
                }

                return messReturn;
            }
        }

        public List<Message> GetMessages(DateTime afterDate)
        {
            using (AttractionsDBContext db = new AttractionsDBContext())
            {
                return db.Messages.Where(a => a.date_time > afterDate).OrderByDescending(a => a.date_time).ToList();
            }
        }

        public string ReturnUserImage(int userId)
        {
            using(AttractionsDBContext db = new AttractionsDBContext())
            {
                return (from u in db.Users where u.Id_user == userId select u.Image).FirstOrDefault(); 
            }
        }

        public User GetUser(int userId)
        {
            using (AttractionsDBContext db = new AttractionsDBContext())
            {
                return db.Users.Where(d => d.Id_user == userId).FirstOrDefault();
            }
        }

        public void AddFriend(int userId, int friendId)
        {
            using (AttractionsDBContext db = new AttractionsDBContext())
            {
                Friend friend = new Friend();
                friend.Id_user = userId;
                friend.Id_user_friend= friendId;
                db.Friends.Add(friend);

                Friend friend1 = new Friend();
                friend1.Id_user = friendId;
                friend1.Id_user_friend = userId;
                db.Friends.Add(friend1);

                db.SaveChanges();
            }
        }

        public int GetScore(int userId)
        {
            using (AttractionsDBContext db = new AttractionsDBContext())
            {
                List<Place> placesId = db.Scores.Where(d => d.Id_user == userId && d.visited == 1).Select(d => d.Place).ToList();

                int score = 0;
                foreach(Place place in placesId)
                {
                    score += Int32.Parse(place.Points.ToString());
                }

                return score;
            }
        }
    }
}