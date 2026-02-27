using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManager.Core.Entities.Identity;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Core.Services.Contract;

namespace TaskManager.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<(ApplicationUser? User, IEnumerable<string> Errors)> RegisterAsync(
    string displayName, string email, string password, string role, Team? team = null)
        {
            // Check duplicate email
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
                return (null, new[] { "Email already registered" });

            var user = new ApplicationUser
            {
                DisplayName = displayName,
                Email = email,
                UserName = email,
                Team = team
            };

            var result = await _userManager.CreateAsync(user, password);

            // If Identity failed, return ALL error messages from Identity
            if (!result.Succeeded)
                return (null, result.Errors.Select(e => e.Description));
            // Why result.Errors? -> Identity gives detailed messages like
            //                       "Password too short", "Invalid email format" etc.

            await _userManager.AddToRoleAsync(user, role);

            return (user, Enumerable.Empty<string>());
        }

        public async Task<ApplicationUser?> LoginAsync(string email, string password)
        {
            // Find user by email
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return null;

            // Check password
            var isCorrect = await _userManager.CheckPasswordAsync(user, password);
            if (!isCorrect) return null;

            return user;
        }

        public string CreateToken(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.DisplayName),
                new Claim(ClaimTypes.Email, user.Email!),
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:DurationInDays"]!)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
