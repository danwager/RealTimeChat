using System.ComponentModel.DataAnnotations;

namespace DataAccess.DataModels
{
    public class User
    {

        [Key]
        public string UserId { get; set; }
        public string Name { get; set; }
        public bool LoggedOut { get; set; }
    }
}