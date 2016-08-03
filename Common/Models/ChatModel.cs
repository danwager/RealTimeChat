using System.Collections.Generic;

namespace Common.Models
{
    public class ChatModel
    {
        public List<User> Users { get; set; }
        public List<Message> Messages { get; set; }
        public bool TooManyUsers { get; set; }
    }
}
