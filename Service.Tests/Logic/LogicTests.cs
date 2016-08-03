using Common.Models;
using DataAccess;
using Logic.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using Data = DataAccess.DataModels;


namespace Tests.Logic
{
    [TestClass]
    public class LogicTests
    {
        ChatLogic _logic;
        private Mock<IChatRepository> _repo;

        [TestInitialize]
        public void SetUp()
        {
            _repo = new Mock<IChatRepository>();

            _logic = new ChatLogic(_repo.Object);
        }

        [TestMethod]
        public void TooManyUsers_ReturnsTrue()
        {
            _repo.Setup(r => r.ActiveUserCount()).Returns(ChatLogic.MAX_USERS).Verifiable();

            Assert.IsTrue(_logic.NewUser(new User()).TooManyUsers);
        }

        [TestMethod]
        public void TooManyUsers_ReturnsFalse()
        {
            _repo.Setup(r => r.ActiveUserCount()).Returns(19).Verifiable();

            Assert.IsFalse(_logic.NewUser(new User()).TooManyUsers);

            _repo.VerifyAll();
        }

        [TestMethod]
        public void NewUser_AddsUser()
        {
            _repo.Setup(r => r.AddUser(It.IsAny<Data.User>())).Verifiable();

            var model = _logic.NewUser(new User());

            _repo.VerifyAll();
        }

        [TestMethod]
        public void UserLoggedOut_SetsLoggedOut()
        {
            _repo.Setup(r => r.LogoutUser(It.Is<Data.User>(mo => mo.LoggedOut == true))).Verifiable();

            _logic.UserLoggedOut(new User());

            _repo.VerifyAll();
        }

        [TestMethod]
        public void NewMessage()
        {
            _repo.Setup(m => m.AddMessage(It.IsAny<Data.Message>())).Verifiable();

            var message = new Message
            {
                User = new User()
            };

            _logic.NewMessage(message);

            _repo.VerifyAll();
        }

        [TestMethod]
        public void GetLatestMessages_ZeroMessages()
        {
            IList<Data.Message> messages = new List<Data.Message>();

            _repo.Setup(m => m.GetMessages()).Returns(messages).Verifiable();

            var model = _logic.NewUser(new User());

            Assert.IsTrue(model.Messages.Count == 0);
            _repo.VerifyAll();
        }

        [TestMethod]
        public void GetLatestMessages_LessThan15Messages()
        {
            IList<Data.Message> messages = new List<Data.Message>();

            for (int i = 1; i < 10; i++)
                messages.Add(new Data.Message
                {
                    User = new Data.User()
                });

            _repo.Setup(m => m.GetMessages()).Returns(messages).Verifiable();

            var model = _logic.NewUser(new User());

            Assert.IsTrue(model.Messages.Count == 9);
            _repo.VerifyAll();
        }

        [TestMethod]
        public void GetLatestMessages_MoreThan15messages()
        {
            IList<Data.Message> messages = new List<Data.Message>();

            for (int i = 1; i < 20; i++)
                messages.Add(new Data.Message
                {
                    User = new Data.User()
                });

            _repo.Setup(m => m.GetMessages()).Returns(messages).Verifiable();

            var model = _logic.NewUser(new User());

            Assert.IsTrue(model.Messages.Count == ChatLogic.MAX_MESSAGES);
            _repo.VerifyAll();
        }

        [TestMethod]
        public void GetActiveUsers()
        {
            _repo.Setup(r => r.GetActiveUsers()).Returns(new List<Data.User>()).Verifiable();


            var model = _logic.NewUser(new User());

            _repo.VerifyAll();
        }

    }
}
