using Common.Models;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Web.Adapters;

namespace Web.Hubs
{
    public class ChatHub : Hub
    {
        private IWepApiAdapter _webApiAdapter;

        public ChatHub()
        {
            _webApiAdapter = new WebApiAdapter();
        }

        public ChatHub(IWepApiAdapter logic)
        {
            _webApiAdapter = logic;
        }

        public void SendMessage(User user, string message)
        {
            Message newMessage = new Message
            {
                MessageText = message,
                User = user
            };

            _webApiAdapter.NewMessage(newMessage);

            Clients.All.newMessage(user, message);
        }

        public void AddNewUser(string connectionId, User user)
        {
            ChatModel model = _webApiAdapter.NewUser(user);

            if (model.TooManyUsers)
            {
                Clients.Client(connectionId).tooManyUsers();

                return;
            }

            Clients.All.addUser(user);
            Clients.Client(connectionId).addAllUsers(model.Users);
            Clients.Client(connectionId).loadPreviousMessages(model.Messages);
        }

        public void UserIsTyping(User user, string message)
        {
            Clients.All.userIsTyping(user, message);
        }

        public void LogoutUser(User user)
        {
            _webApiAdapter.UserLoggedOut(user);

            Clients.All.userLoggedOut(user);
        }

        public override Task OnConnected()
        {
            Clients.Client(Context.ConnectionId).signalConnectionId(Context.ConnectionId);

            return base.OnConnected();
        }
    }
}