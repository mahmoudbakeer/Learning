using ControllerProjectManagement.Entities;
using ControllerProjectManagement.Enums;

namespace ControllerProjectManagement.Responses;

public class TaskResponse
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Guid ProjectId { get; set; }
    public Guid AssignedUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public ProjectTaskStatus Status { get; set; }

    public static TaskResponse FromModel(ProjectTask task)
    {
        return new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            ProjectId = task.ProjectId,
            AssignedUserId = task.AssignedUserId,
            CreatedAt = task.CreatedAt,
            Status = task.Status
        };
    }
}