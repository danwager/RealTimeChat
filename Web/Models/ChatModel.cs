using System.Collections.Generic;

namespace Web.Models
{
    public class ChatModel
    {
        public IList<User> Users { get; set; }
        public IList<Message> Messages { get; set; }
    }
}