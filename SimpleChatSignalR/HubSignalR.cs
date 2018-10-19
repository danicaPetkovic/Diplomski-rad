using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using MongoDB.Bson;
using Baza;
using SimpleChatSignalR.Models;
using Baza.DataLayer;
using System.Threading.Tasks;

namespace SimpleChatSignalR
{
    public class HubSignalR : Hub
    {
        private readonly static ConnectionMapping<string> Connections = ConnectionMapping<string>.GetConection;

        public void SendMessage(string jsonMessageSenderObject)
        {
            //string name = Context.QueryString["id"].ToString();

            MessageView msgsnd = JsonConvert.DeserializeObject<MessageView>(jsonMessageSenderObject);

            Message msg = new Message();
            msg.message1 = msgsnd.message;
            msg.date_time = DateTime.Now;
            msg.Id_send_user = msgsnd.Id_send_user;
            msg.Id_recv_user = msgsnd.Id_recv_user;

            UserDL.Data.AddMessage(msg);
            msgsnd.date_time = DateTime.Now;
            //Clients.All.newMessage(JsonConvert.SerializeObject(msgsnd));

            var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            

            string userId = Context.QueryString["userId"].ToString();

            foreach (var connectionId in Connections.GetConnections(userId))
            {
                Clients.Client(connectionId).newMessage(JsonConvert.SerializeObject(msgsnd));
            }

            foreach (var connectionId in Connections.GetConnections(msg.Id_recv_user.ToString()))
            {
                Clients.Client(connectionId).newMessage(JsonConvert.SerializeObject(msgsnd));

                notificationHub.Clients.Client(connectionId).notify(JsonConvert.SerializeObject(msgsnd));
            }

        }

        public override Task OnConnected()
        {
            string userId = Context.QueryString["userId"].ToString();

            Connections.Add(userId, Context.ConnectionId);

            return base.OnConnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            string userId = Context.QueryString["userId"].ToString();

            Connections.Remove(userId, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string userId = Context.QueryString["userId"].ToString();

            Connections.Add(userId, Context.ConnectionId);

            return base.OnReconnected();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                using (AttractionsDBContext db = new AttractionsDBContext())
                {
                    db.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}