namespace TaskManager.APIs.Dto.AuthDtos
{
    public class UserReturnDto
    {
        public string? Id { get; set; }
        public string DisplayName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
