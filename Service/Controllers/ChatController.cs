using Common.Models;
using Logic.Logic;
using System.Web.Http;

namespace Service.Controllers
{
    public class ChatController : ApiController
    {
        private IChatLogic _logic = new ChatLogic();

        [HttpPost]
        public void NewMessage(Message message)
        {
            _logic.NewMessage(message);
        }

        [HttpPost]
        public ChatModel NewUser(User user)
        {
            return _logic.NewUser(user);
        }

        [HttpPost]
        public void UserLoggedOut(User user)
        {
            _logic.UserLoggedOut(user);
        }

    }
}
