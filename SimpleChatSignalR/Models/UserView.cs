using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Baza;

namespace SimpleChatSignalR.Models
{
    public class UserView
    {
        public int Id_user { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone_number { get; set; }
        public string Image { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Points { get; set; }

        public UserView() { }

        public UserView(User u)
        {
            Id_user = u.Id_user;
            Name = u.Name;
            Surname = u.Surname;
            Phone_number = u.Phone_number;
            Image = u.Image;
            Username = u.Username;
            Password = u.Password;
        }
    }
}