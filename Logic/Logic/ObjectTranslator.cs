using Data = DataAccess.DataModels;
using Common = Common.Models;

namespace Logic.Logic
{
    public class ObjectTranslator
    {
        public static Data.User GetDbUser(global::Common.Models.User user)
        {
            return new Data.User
            {
                UserId = user.Id,
                Name = user.Name
            };

        }

        public static Data.Message GetDbMessage(global::Common.Models.Message message)
        {
            return new Data.Message
            {
                //User = GetDbUser(message.User),
                MessageText = message.MessageText,
                UserId = message.User.Id
            };
        }

        public static global::Common.Models.Message GetWebMessage(Data.Message message)
        {
            return new global::Common.Models.Message
            {
                User = GetWebUser(message.User),
                MessageText = message.MessageText
            };
        }

        public static global::Common.Models.User GetWebUser(Data.User user)
        {
            return new global::Common.Models.User
            {
                Id = user.UserId,
                Name = user.Name
            };
        }
    }
}
