using System.ComponentModel.DataAnnotations;


namespace CMCSWeb.Models
{
    public class Lecturer
    {
        [Key]
        public int LecturerId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Phone]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(100)]
        public string? Department { get; set; }

        [StringLength(100)]
        public string? ModuleTaught { get; set; }

        public decimal HourlyRate { get; set; }

        // Navigation property – a lecturer can submit many claims
        public ICollection<Claim>? Claims { get; set; }
    }
}

