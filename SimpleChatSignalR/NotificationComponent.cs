using Baza;
using Baza.DataLayer;
using Microsoft.AspNet.SignalR;
using SimpleChatSignalR.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SimpleChatSignalR
{
    public class NotificationComponent
    {
        public void RegisterNotification(DateTime currentTime)
        {
            string conStr = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;
            string sqlCommand = @"SELECT [Id_message], [Id_send_user], [ID_recv_user], [message], [date_time] from [dbo].[Message] where [date_time] > @AddedOn";

            using (SqlConnection con = new SqlConnection())
            {
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                cmd.Parameters.AddWithValue("@AddedOn", currentTime);

                if(con.State != System.Data.ConnectionState.Open)
                {
                    con.ConnectionString = conStr;
                    con.Open();
                }

                cmd.Notification = null;
                SqlDependency sqlDependency = new SqlDependency(cmd);
                sqlDependency.OnChange += sqlDependency_OnChange;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                }
            }
        }   

        void sqlDependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
           /* if(e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDependency = sender as SqlDependency;
                sqlDependency.OnChange -= sqlDependency_OnChange;

                var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                notificationHub.Clients.All.notify("added");

                RegisterNotification(DateTime.Now);
            }*/
        }

        public List<MessageView> GetMessages(DateTime afterDate)
        {
            List<Message> messages = UserDL.Data.GetMessages(afterDate);
            List<MessageView> messageViews = new List<MessageView>();
            foreach(Message message in messages)
            {
                messageViews.Add(new MessageView(message));
            }

            return messageViews;
        }
    }
}