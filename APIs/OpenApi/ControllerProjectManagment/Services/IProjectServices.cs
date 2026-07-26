using ControllerProjectManagement.Requests;
using ControllerProjectManagement.Responses;

namespace ControllerProjectManagement.Services;

public interface IProjectServices
{
    Task AssignUserToTaskAsync(Guid ProjectId, Guid TaskId, Guid UserId, Guid CurrentUserId);
    Task<ProjectResponse> CreateProjectAsync(CreateProjectRequest createProjectRequest, Guid CurrentUserId);
    Task<TaskResponse> CreateProjectTaskAsync(Guid ProjectId, CreateTaskRequest taskRequest, Guid CurrentUserId);
    Task<bool> DeleteProjectAsync(Guid ProjectId, Guid CurrentUserId);
    Task<bool> DeleteProjectTaskAsync(Guid ProjectId, Guid TaskId, Guid CurrentUserId);
    Task EndProjectAsync(Guid ProjectId, Guid CurrentUserId);
    Task<ProjectResponse> GetProjectAsync(Guid ProjectId);
    Task<List<ProjectResponse>> GetProjectsAsync();
    Task<TaskResponse> GetTaskAsync(Guid ProjectId, Guid TaskId);
    Task<bool> UpdateProjectAsync(Guid ProjectId, UpdateProjectRequest updateProjectRequest, Guid CurrentUserId);
    Task<bool> UpdateProjectBudgetAsync(Guid ProjectId, UpdateBudgetRequest updateBudgetRequest, Guid CurrentUserId);
    Task<bool> UpdateTaskAsync(Guid ProjectId, Guid TaskId, UpdateTaskRequest updateTaskRequest, Guid CurrentUserId);
    Task<bool> UpdateTaskStatusCodeAsync(Guid ProjectId, Guid TaskId, UpdateTaskStatusRequest StatusRequest, Guid CurrentUserId);
}




