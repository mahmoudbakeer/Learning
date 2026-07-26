namespace ControllerProjectManagement.Requests;

public class UpdateProjectRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime ExpectedStartDate { get; set; }
}