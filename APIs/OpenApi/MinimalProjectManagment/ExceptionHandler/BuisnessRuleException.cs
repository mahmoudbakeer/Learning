namespace ControllerProjectManagement.ExceptionHandler;


public class BusinessRuleException : Exception
{
    public int StatusCode { get; set; }
    public BusinessRuleException(string message, int statusCode) : base(message)
    {
        this.StatusCode = statusCode;
    }
}