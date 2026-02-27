using Microsoft.AspNetCore.Identity;
using TaskManager.Core.Entities.Tasks;

namespace TaskManager.Core.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; } = null!;
        public Team? Team { get; set; }
    }
}
