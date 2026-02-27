using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.APIs.Dto.AuthDtos;
using TaskManager.APIs.Errors;
using TaskManager.Core.Entities.Identity;
using TaskManager.Core.Services.Contract;

namespace TaskManager.APIs.Controllers.Account
{
    public class AccountController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(IAuthService authService, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        // POST /api/account/login
        // Public - no auth required
        [HttpPost("login")]
        public async Task<ActionResult<UserReturnDto>> Login([FromBody] LoginDto loginDto)
        {
            var user = await _authService.LoginAsync(loginDto.Email, loginDto.Password);
            if (user is null)
                return Unauthorized(new ApiResponse(401, "Invalid email or password"));

            var roles = await _userManager.GetRolesAsync(user);
            var token = _authService.CreateToken(user, roles);

            return Ok(new UserReturnDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Role = roles.FirstOrDefault() ?? string.Empty,
                Token = token
            });
        }

        // POST /api/account/register
        // Admin only - only admin can create new users
        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserReturnDto>> Register([FromBody] RegisterDto registerDto)
        {
            var validRoles = new[] { "Admin", "TeamLeader", "TeamMember" };
            if (!validRoles.Contains(registerDto.Role))
                return BadRequest(new ApiResponse(400,
                    $"Invalid role. Valid roles: {string.Join(", ", validRoles)}"));

            var (user, errors) = await _authService.RegisterAsync(
                registerDto.DisplayName,
                registerDto.Email,
                registerDto.Password,
                registerDto.Role,
                registerDto.Team
            );

            if (user is null)
                return BadRequest(new ApiValidationResponse { Errors = errors.ToList() });

            var roles = await _userManager.GetRolesAsync(user);
            var token = _authService.CreateToken(user, roles);

            return Ok(new UserReturnDto
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Role = roles.FirstOrDefault() ?? string.Empty,
                Token = token
            });
        }

        // GET /api/account/users
        // Admin → returns all TeamLeaders + TeamMembers
        // TeamLeader → returns TeamMembers only (for task assignment)
        [HttpGet("users")]
        [Authorize(Roles = "Admin,TeamLeader")]
        public async Task<ActionResult<IEnumerable<UserReturnDto>>> GetUsers()
        {
            var isAdmin = User.IsInRole("Admin");
            List<ApplicationUser> users = new();

            if (isAdmin)
            {
                // Admin sees TeamLeaders and TeamMembers
                var leaders = await _userManager.GetUsersInRoleAsync("TeamLeader");
                var members = await _userManager.GetUsersInRoleAsync("TeamMember");
                users.AddRange(leaders);
                users.AddRange(members);
                // Why not include Admins? -> Admins are not assignable to tasks
                //                            no need to show them in the list
            }
            else
            {
                // TeamLeader sees only members to assign tasks to
                var members = await _userManager.GetUsersInRoleAsync("TeamMember");
                users.AddRange(members);
            }

            // Build result - get role per user
            var result = new List<UserReturnDto>();
            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                result.Add(new UserReturnDto
                {
                    Id = u.Id,
                    DisplayName = u.DisplayName,
                    Email = u.Email!,
                    Role = roles.FirstOrDefault() ?? string.Empty,
                    Token = string.Empty
                    // Why empty token? -> This is a lookup list only
                    //                     tokens are only issued on login
                });
            }

            return Ok(result);
        }
    }
}
