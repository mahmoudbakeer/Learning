namespace CQRSInAction.API.Requests;

public class UpdateTodoRequest
{
    public string Title { get; set; } = string.Empty;
    public bool Completed { get; set; }
}