using ControllerProjectManagement.Enums;

namespace ControllerProjectManagement.Requests;


public class UpdateTaskStatusRequest
{
    public ProjectTaskStatus Status { get; set; }
}