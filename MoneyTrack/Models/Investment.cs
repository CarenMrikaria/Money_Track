using MoneyTrack.Areas.Identity.Data;

namespace MoneyTrack.Models
{
    public class Investment
    {
        public int InvestmentId { get; set; }
        public string UserId { get; set; }  // Foreign key to the user (AspNetUsers)
        public int ProjectId { get; set; }  // Foreign key to the project
        public decimal Amount { get; set; }
        public DateTime InvestmentDate { get; set; }

        public MoneyTrackUser User { get; set; }  // Navigation property to the user
        public Project Project { get; set; }  // Navigation property to the project
    }

}
