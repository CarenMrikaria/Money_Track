using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace MoneyTrack.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }         
        public string? ProjectName { get; set; }           
        public string? ProjectDescription { get; set; }   
        public string? Location { get; set; }
        public string? Type { get; set; }
        public decimal Target { get; set; }
        public decimal currentAmount { get; set; } = 0;
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; }
        public string? Status { get; set; } = "In Progress";

    }
}
