using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Baza;
using Baza.DataLayer;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using SimpleChatSignalR.Models;

namespace SimpleChatSignalR
{
    public class NotificationHub : Hub
    {
        public readonly static ConnectionMapping<string> Connections = ConnectionMapping<string>.GetConection;
        public readonly static List<UserPosition> positions = new List<UserPosition>();

        public void setPosition(string lat, string lng)
        {
            string userId = Context.QueryString["userId"].ToString();

            UserPosition position = positions.Where(d => d.Id_user == Int32.Parse(userId)).FirstOrDefault();

            if(position == null)
            {
                User user = UserDL.Data.GetUser(Int32.Parse(userId));

                position = new UserPosition(user);
                position.Latitude = float.Parse(lat);
                position.Longitude = float.Parse(lng);

                positions.Add(position);
            }
        }

        public void updateLocation(string userId, string lat, string lng)
        {
            positions.Where(d => d.Id_user == Int32.Parse(userId)).First().Latitude = float.Parse(lat);
            positions.Where(d => d.Id_user == Int32.Parse(userId)).First().Longitude = float.Parse(lng);
        }

        public void updateFriendsLocations(string userId, string lat, string lng)
        {
            positions.Where(d => d.Id_user == Int32.Parse(userId)).First().Latitude = float.Parse(lat);
            positions.Where(d => d.Id_user == Int32.Parse(userId)).First().Longitude = float.Parse(lng);

            List<UserPosition> pos = new List<UserPosition>();
            pos = positions;

            foreach (var connectionId in Connections.GetConnections(userId))
            {
                Clients.Client(connectionId).updateUsersPositions(JsonConvert.SerializeObject(pos));
            }
        }

        public void friendRequest(string id)
        {
            string userId = Context.QueryString["userId"].ToString();

            foreach (var connectionId in Connections.GetConnections(id))
            {
                Clients.Client(connectionId).reciveFriendRequest(JsonConvert.SerializeObject(userId));
            }
        }

        public override Task OnConnected()
        {
            string userId = Context.QueryString["userId"].ToString();

            Connections.Add(userId, Context.ConnectionId);

            return base.OnConnected();
        }

        public ConnectionMapping<string> MyConnection()
        {
            return Connections;
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