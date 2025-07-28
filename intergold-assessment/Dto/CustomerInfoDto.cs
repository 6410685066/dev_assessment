namespace InterGoldAssessment.Models
{
    public class CustomerInfoDto
    {
        public int Id { get; set; }

        public string FirstNameName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime? LastLogin { get; set; }

        public string Status { get; set; } = "A";
    }
}
