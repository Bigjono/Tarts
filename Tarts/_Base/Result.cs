namespace Tarts._Base
{
    public class Result1
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public Result1(bool success = true, string message = "")
        {
            Success = success;
            Message = message;
        }
    }
}
