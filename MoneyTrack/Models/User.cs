using System.ComponentModel.DataAnnotations;

namespace MoneyTrack.Models
{
    public class User
    {
        [Key] 
        public int UserID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public DateTime DateRegistered { get; set; }
        
        
    }
}
