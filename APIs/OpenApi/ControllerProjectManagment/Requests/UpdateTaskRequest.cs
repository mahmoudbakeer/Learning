using ControllerProjectManagement.Enums;
namespace ControllerProjectManagement.Requests;

public class UpdateTaskRequest
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }
}