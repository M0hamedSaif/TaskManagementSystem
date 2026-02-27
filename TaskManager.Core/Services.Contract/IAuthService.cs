using TaskManager.Core.Entities.Identity;
using TaskManager.Core.Entities.Tasks;

namespace TaskManager.Core.Services.Contract
{
    public interface IAuthService
    {
        string CreateToken(ApplicationUser user, IList<string> roles);
        Task<ApplicationUser?> LoginAsync(string email, string password);
        Task<(ApplicationUser? User, IEnumerable<string> Errors)> RegisterAsync(
            string displayName, string email, string password, string role, Team? team = null);
    }
}
