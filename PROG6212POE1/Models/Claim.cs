using System;
using System.ComponentModel.DataAnnotations;

namespace CMCSWeb.Models
{
    public enum ClaimStatus
    {
        Pending,     // Submitted by Lecturer, awaiting coordinator
        Verified,    // Approved by Coordinator, awaiting Manager
        Approved,    // Approved by Manager
        Rejected     // Rejected by Coordinator or Manager
    }

    public class Claim
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Lecturer Name is required.")]
        [Display(Name = "Lecturer Name")]
        public required string LecturerName { get; set; }

        [Required(ErrorMessage = "Hours Worked is required.")]
        [Range(0.1, 500, ErrorMessage = "Hours Worked must be greater than 0.")]
        public double HoursWorked { get; set; }

        [Required(ErrorMessage = "Hourly Rate is required.")]
        [Range(0.1, 10000, ErrorMessage = "Hourly Rate must be greater than 0.")]
        public double HourlyRate { get; set; }

        [Display(Name = "Notes")]
        [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public required string Notes { get; set; }

        [Display(Name = "Uploaded Document Path")]
        public  string? DocumentPath { get; set; }  // Stores filename for uploaded documents

        [Display(Name = "Status")]
        public ClaimStatus Status { get; set; } = ClaimStatus.Pending;

        [Display(Name = "Submitted At")]
        public DateTime SubmittedAt { get; set; } = DateTime.Now; // Timestamp of submission
    }
}
