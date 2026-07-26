using ControllerProjectManagement.Entities;

namespace ControllerProjectManagement.Responses;


public class ProjectResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Guid OwnerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpectedStartDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public decimal Budget { get; set; }
    public string? Currency { get; set; }
    public List<TaskResponse> Tasks { get; set; } = [];




    public static ProjectResponse FromModel(Project project)

    {
        return new ProjectResponse
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            OwnerId = project.OwnerId,
            CreatedAt = project.CreatedAt,
            ExpectedStartDate = project.ExpectedStartDate,
            ActualEndDate = project.ActualEndDate,
            Budget = project.Budget,
            Tasks = project.Tasks.Select(TaskResponse.FromModel).ToList()
        };
    }
}