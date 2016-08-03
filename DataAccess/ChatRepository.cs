using DataAccess.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess
{
    public interface IChatRepository : IDisposable
    {
        IEnumerable<Message> GetMessages();
        IEnumerable<User> GetActiveUsers();
        void AddUser(User user);
        void LogoutUser(User user);
        void AddMessage(Message message);
        int ActiveUserCount();
    }

    public class ChatRepository : IChatRepository
    {
        private ChatContext _context = new ChatContext();

        public void AddMessage(Message message)
        {
            try
            {
                _context.Messages.Add(message);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddUser(User user)
        {
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public int ActiveUserCount()
        {
            return _context.Users.Where(u => u.LoggedOut == false).Count();
        }

        public IEnumerable<User> GetActiveUsers()
        {
            return _context.Users.Where(u => u.LoggedOut == false).ToList();
        }

        public void Dispose()
        {
            _context = null;
        }

        public IEnumerable<Message> GetMessages()
        {
            return _context.Messages;
        }

        public void LogoutUser(User user)
        {
            try
            {
                _context.Users.Attach(user);
                var entry = _context.Entry(user);

                entry.Property(e => e.LoggedOut).IsModified = true;

                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
