using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Baza;

namespace SimpleChatSignalR.Models
{
    public class MessageView
    {
        public int Id_message { get; set; }
        public Nullable<int> Id_send_user { get; set; }
        public Nullable<int> Id_recv_user { get; set; }
        public string message { get; set; }
        public Nullable<System.DateTime> date_time { get; set; }

        public MessageView() { }

        public MessageView(Message message)
        {
            Id_message = message.Id_message;
            Id_send_user = message.Id_send_user;
            Id_recv_user = message.Id_recv_user;
            this.message = message.message1;
            date_time = message.date_time;
        }
    }
}