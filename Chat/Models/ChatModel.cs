using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR.Hubs;
using Chat.Hubs;

namespace Chat.Models
{
    public class ChatModel
    {
        private readonly static Lazy<ChatModel> _instance = new Lazy<ChatModel>(() => new ChatModel());
        public static List<Client> Clients = new List<Client>();

        public static ChatModel Instance
        {
            get
            {
                return _instance.Value;
            }
        }
    }
}