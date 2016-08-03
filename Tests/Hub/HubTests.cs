using Common.Models;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using Web.Adapters;
using Web.Hubs;

namespace SkarplineCodeProject.Tests.Hub
{
    [TestClass]
    public class HubTests
    {
        private ChatHub _chatHub;
        private Mock<IHubCallerConnectionContext<dynamic>> _mockClients;
        private Mock<HubCallerContext> _mockContext;
        private Mock<IClientContract> _clientContract;
        private Mock<IWepApiAdapter> _adapter;

        [TestInitialize]
        public void SetUp()
        {
            _adapter = new Mock<IWepApiAdapter>();

            _chatHub = new ChatHub(_adapter.Object);
            _mockClients = new Mock<IHubCallerConnectionContext<dynamic>>();
            _mockContext = new Mock<HubCallerContext>();
            _clientContract = new Mock<IClientContract>();

            _chatHub.Clients = _mockClients.Object;

            _mockClients.Setup(m => m.All).Returns(_clientContract.Object);
            _mockClients.Setup(m => m.Client(It.IsAny<string>())).Returns(_clientContract.Object);
            _mockContext.SetupAllProperties();
        }

        [TestMethod]
        public void SendMessageIsCalled()
        {
            _clientContract.Setup(m => m.newMessage(It.IsAny<User>(), It.IsAny<string>())).Verifiable();

            _adapter.Setup(l => l.NewMessage(It.IsAny<Message>())).Verifiable();

            _chatHub.SendMessage(new User(), "Test Message");

            _clientContract.VerifyAll();
            _adapter.VerifyAll();
        }

        [TestMethod]
        public void AddNewUser_NotTooManyUsers_AddsUsers_LoadsUsersAndMessages()
        {
            _clientContract.Setup(m => m.addAllUsers(It.IsAny<List<User>>())).Verifiable();
            _clientContract.Setup(m => m.loadPreviousMessages(It.IsAny<List<Message>>())).Verifiable();

            ChatModel model = new ChatModel
            {
                TooManyUsers = false,
                Messages = new List<Message>(),
                Users = new List<User>()
            };

            _adapter.Setup(l => l.NewUser(It.IsAny<User>())).Returns(model).Verifiable();

            _chatHub.AddNewUser("", new User());

            _clientContract.VerifyAll();
            _adapter.VerifyAll();
        }

        [TestMethod]
        public void AddNewUser_TooManyUsers_CallsTooManyUsers()
        {
            _clientContract.Setup(m => m.tooManyUsers()).Verifiable();

            _adapter.Setup(l => l.NewUser(It.IsAny<User>())).Returns(new ChatModel { TooManyUsers = true }).Verifiable();

            _chatHub.AddNewUser("", new User());

            _clientContract.VerifyAll();
            _adapter.VerifyAll();
        }

        [TestMethod]
        public void UserIsTyping()
        {
            _clientContract.Setup(m => m.userIsTyping(It.IsAny<User>(), It.IsAny<string>())).Verifiable();

            _chatHub.UserIsTyping(new User(), "");

            _clientContract.VerifyAll();
            _adapter.VerifyAll();
        }

        [TestMethod]
        public void UserLoggedOut()
        {
            _clientContract.Setup(m => m.userLoggedOut(It.IsAny<User>())).Verifiable();

            _chatHub.LogoutUser(new User());

            _clientContract.VerifyAll();
            _adapter.VerifyAll();
        }

        [TestMethod]
        public void OnConnected()
        {
            _clientContract.Setup(m => m.signalConnectionId(It.IsAny<string>())).Verifiable();
            _chatHub.Context = _mockContext.Object;

            var data = _chatHub.OnConnected();

            _clientContract.VerifyAll();
        }


        public interface IClientContract
        {
            void newMessage(User user, string message);
            void addAllUsers(IList<User> users);
            void loadPreviousMessages(IList<Message> messages);
            void tooManyUsers();
            void userIsTyping(User user, string message);
            void userLoggedOut(User user);
            void signalConnectionId(string id);
        }
    }
}
