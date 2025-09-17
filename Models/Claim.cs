using System.ComponentModel.DataAnnotations;

namespace CMCSWeb.Models
{
    public class Claim
    {
        public int ClaimId { get; set; }       // Primary Key
        public int LecturerId { get; set; }    // Foreign Key to Lecturer

        [Required]
        [Range(1, 200)]
        public decimal HoursWorked { get; set; }

        [Required]
        [Range(100, 2000)]
        public decimal HourlyRate { get; set; }

        // Calculated property (not stored in DB directly)
        public decimal TotalAmount => HoursWorked * HourlyRate;

        // New fields for prototype
        public string? SupportingDocumentPath { get; set; } // uploaded file
        public bool IsVerified { get; set; } = false;       // Programme Coordinator
        public bool IsApproved { get; set; } = false;       // Academic Manager

        [Required]
        public string Status { get; set; } = "Pending";     // Pending, Verified, Approved, Settled
    }
}
