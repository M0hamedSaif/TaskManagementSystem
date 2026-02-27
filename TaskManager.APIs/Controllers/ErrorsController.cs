using Microsoft.AspNetCore.Mvc;
using TaskManager.APIs.Errors;

namespace TaskManager.APIs.Controllers
{
    [Route("error/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        public IActionResult HandleError(int code)  // ← renamed from Error to HandleError
        {
            return code switch
            {
                401 => Unauthorized(new ApiResponse(code)),
                403 => StatusCode(403, new ApiResponse(code)),  
                404 => NotFound(new ApiResponse(code)),
                _ => StatusCode(code)
            };
        }
    }
}
