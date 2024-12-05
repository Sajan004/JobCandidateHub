using System.ComponentModel.DataAnnotations;

namespace JobCandidate.Domain.DomainClasses
{
    public class Candidate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty!;

        [Required]
        public string LastName { get; set; } = string.Empty!;

        public string? PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? CallTimeInterval { get; set; }

        public string? LinkedInProfile { get; set; }

        public string? GitHubProfile { get; set; }

        [Required]
        public string Comment { get; set; } = string.Empty;
    }
}
