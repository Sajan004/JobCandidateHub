using System.ComponentModel.DataAnnotations;

namespace JobCandidate.Domain.DomainClasses
{
    public class Candidate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        public string FirstName { get; set; } = default!;

        [Required]
        public string LastName { get; set; } = default!;

        public string? PhoneNumber { get; set; }

        public string? BestCallTime { get; set; }

        public string? LinkedInProfile { get; set; }

        public string? GitHubProfile { get; set; }

        [Required]
        public string Comment { get; set; } = default!;
    }
}
