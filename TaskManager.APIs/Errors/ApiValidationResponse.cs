namespace TaskManager.APIs.Errors
{
    public class ApiValidationResponse : ApiResponse
    {
        public List<string> Errors { get; set; } = new();

        public ApiValidationResponse() : base(400, "Validation Error")
        {
        }
    }
}
