using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.DataModels
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public string MessageText { get; set; }

        public virtual User User { get; set; }
    }
}