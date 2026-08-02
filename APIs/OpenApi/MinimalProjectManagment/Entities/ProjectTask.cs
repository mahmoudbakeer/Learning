using ControllerProjectManagement.Enums;

namespace ControllerProjectManagement.Entities;

public class ProjectTask
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public Guid ProjectId { get; set; }
    public Project Project { get; set; }
    public Guid AssignedUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public ProjectTaskStatus Status { get; set; }
}
