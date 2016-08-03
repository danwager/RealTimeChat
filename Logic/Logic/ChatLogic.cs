using Common.Models;
using DataAccess;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Logic
{
    public interface IChatLogic
    {
        ChatModel NewUser(User user);
        void UserLoggedOut(User user);
        void NewMessage(Message message);
    }

    public class ChatLogic : IChatLogic
    {
        public const int MAX_USERS = 20;
        public const int MAX_MESSAGES = 15;

        private IChatRepository _chatRepo;
        public ChatLogic()
        {
            _chatRepo = new ChatRepository();
        }

        public ChatLogic(IChatRepository repo)
        {
            _chatRepo = repo;
        }

        public ChatModel NewUser(User user)
        {
            ChatModel model = new ChatModel();

            if (_chatRepo.ActiveUserCount() >= MAX_USERS)
            {
                model.TooManyUsers = true;
                model.Messages = new List<Message>();
                model.Users = new List<User>();

                return model;
            }

            _chatRepo.AddUser(ObjectTranslator.GetDbUser(user));

            model.Messages = GetLatestMessages().ToList();
            model.Users = GetActiveUsers().ToList();

            return model;
        }

        public void UserLoggedOut(User user)
        {
            var loggedOutUser = ObjectTranslator.GetDbUser(user);

            loggedOutUser.LoggedOut = true;

            _chatRepo.LogoutUser(loggedOutUser);
        }

        public void NewMessage(Message message)
        {
            _chatRepo.AddMessage(ObjectTranslator.GetDbMessage(message));
        }

        private IList<Message> GetLatestMessages()
        {
            var messages = _chatRepo.GetMessages().ToList();

            if (messages.Count == 0)
                return new List<Message>();

            if (messages.Count <= MAX_MESSAGES)
                return messages.Select(ObjectTranslator.GetWebMessage).ToList();

            return messages.Select(ObjectTranslator.GetWebMessage).Skip(messages.Count - MAX_MESSAGES).Take(MAX_MESSAGES).ToList();
        }

        private IList<User> GetActiveUsers()
        {
            var users = _chatRepo.GetActiveUsers().ToList();

            return users.Select(ObjectTranslator.GetWebUser).ToList();
        }

        private bool TooManyUsers()
        {
            return _chatRepo.ActiveUserCount() >= 20;
        }
    }
}
