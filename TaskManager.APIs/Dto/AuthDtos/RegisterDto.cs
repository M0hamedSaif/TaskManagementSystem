using System.ComponentModel.DataAnnotations;
using TaskManager.Core.Entities.Tasks;

namespace TaskManager.APIs.Dto.AuthDtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Display name is required")]
        public string DisplayName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; } = null!;
        public Team? Team { get; set; }
    }
}
