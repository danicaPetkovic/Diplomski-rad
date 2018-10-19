using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Baza;
using Baza.DataLayer;

namespace SimpleChatSignalR.Models
{
    public class CommentView
    {
        public int Id_comment { get; set; }
        public int? Id_place { get; set; }
        public string Comment { get; set; }
        public string Type { get; set; }

        public UserView User { get; set; }

        public CommentView() { }

        public CommentView(Comment c)
        {
            Id_comment = c.Id_comment;
            Id_place = c.Id_place;
            Comment = c.Comment1;
            Type = c.Type;
            if(c.Id_user != null)
            {
                User = new UserView(UserDL.Data.GetUser(Int32.Parse(c.Id_user.ToString())));
            }
        }
    }
}