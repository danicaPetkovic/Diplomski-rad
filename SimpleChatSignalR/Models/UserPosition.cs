using Baza;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleChatSignalR.Models
{
    public class UserPosition
    {
        public int Id_user { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Image { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public UserPosition() { }

        public UserPosition(User u)
        {
            Id_user = u.Id_user;
            Name = u.Name;
            Surname = u.Surname;
            Image = u.Image;
        }
    }
}