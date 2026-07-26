using ControllerProjectManagement.Data;
using ControllerProjectManagement.Entities;
using ControllerProjectManagement.Enums;
using ControllerProjectManagement.ExceptionHandler;
using ControllerProjectManagement.Requests;
using ControllerProjectManagement.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ControllerProjectManagement.Services;



public class ProjectServices(AppDbContext dbContext) : IProjectServices
{
    public async Task<ProjectResponse> GetProjectAsync(Guid ProjectId)
    {
        var project = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == ProjectId);

        if (project is null)
            throw new BusinessRuleException(
                $"The Project with ProjectId = {ProjectId} Not Found",
                StatusCodes.Status404NotFound
            );
        else
            return ProjectResponse.FromModel(project);
    }
    public async Task<List<ProjectResponse>> GetProjectsAsync()
    {
        var projects = await dbContext.Projects.Include(p => p.Tasks).ToListAsync();
        return projects.Select(p => ProjectResponse.FromModel(p)).ToList();
    }
    public async Task<TaskResponse> GetTaskAsync(Guid ProjectId, Guid TaskId)
    {
        var project = await dbContext.Projects.Include(p => p.Tasks).FirstOrDefaultAsync(p => p.Id == ProjectId);
        if (project is null)
            throw new BusinessRuleException(
                $"The Project with ProjectId = {ProjectId} Not Found",
                StatusCodes.Status404NotFound
            );
        var task = project.Tasks.FirstOrDefault(t => t.Id == TaskId);

        if (task is null)
            throw new BusinessRuleException(
                $"The Task with TaskId = {TaskId} Not Found",
                StatusCodes.Status404NotFound
            );
        else
        {
            return TaskResponse.FromModel(task);
        }
    }

    public async Task<bool> UpdateProjectAsync(Guid ProjectId, UpdateProjectRequest updateProjectRequest, Guid CurrentUserId)
    {
        var project = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == ProjectId);


        if (project is null)
            throw new BusinessRuleException(
                $"The Project with ProjectId = {ProjectId} Not Found",
                StatusCodes.Status404NotFound
            );
        else if (CurrentUserId != project.OwnerId)
            throw new BusinessRuleException(
               $"Only the Owner of project can updated it.",
               StatusCodes.Status403Forbidden
           );
        else
        {
            project.Name = updateProjectRequest.Name;
            project.Description = updateProjectRequest.Description ?? project.Description;
            project.ExpectedStartDate = updateProjectRequest.ExpectedStartDate;

            int value = await dbContext.SaveChangesAsync();
            return value > 0;
        }
    }

    public async Task<bool> UpdateTaskAsync(
        Guid ProjectId,
        Guid TaskId,
        UpdateTaskRequest updateTaskRequest,
        Guid CurrentUserId
    )
    {
        var project = await dbContext.Projects.Include(p => p.Tasks).FirstOrDefaultAsync(p => p.Id == ProjectId);
        if (project is null)
            throw new BusinessRuleException(
                $"The Project with ProjectId = {ProjectId} Not Found",
                StatusCodes.Status404NotFound
            );
        var task = project?.Tasks.FirstOrDefault(t => t.Id == TaskId);

        if (task is null)
            throw new BusinessRuleException(
                $"The Task with TaskId = {TaskId} Not Found",
                StatusCodes.Status404NotFound
            );
        if (project?.OwnerId != CurrentUserId && task?.AssignedUserId != CurrentUserId)
        {
            throw new BusinessRuleException($"Only the assigned user or the Project Owner can update the task.", StatusCodes.Status403Forbidden);
        }
        else
        {



            task.Title = updateTaskRequest.Title;
            task.Description = updateTaskRequest.Description ?? task.Description;
            int value = await dbContext.SaveChangesAsync();
            return value > 0;

        }
    }
    public async Task<ProjectResponse> CreateProjectAsync(CreateProjectRequest createProjectRequest, Guid CurrentUserId)
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = createProjectRequest.Name,
            Budget = createProjectRequest.Budget,
            Description = createProjectRequest.Description,
            ExpectedStartDate = createProjectRequest.ExpectedStartDate,
            CreatedAt = DateTime.UtcNow,
            OwnerId = CurrentUserId
        };

        dbContext.Projects.Add(project);

        await dbContext.SaveChangesAsync();

        return ProjectResponse.FromModel(project);
    }
    public async Task EndProjectAsync(Guid ProjectId, Guid CurrentUserId)
    {
        var project = await dbContext.Projects.Include(p => p.Tasks).FirstOrDefaultAsync(p => p.Id == ProjectId);
        if (project is null)
            throw new BusinessRuleException(
                $"The Project with ProjectId = {ProjectId} Not Found",
                StatusCodes.Status404NotFound
            );
        if (project.ActualEndDate.HasValue)
        {
            throw new BusinessRuleException(
               $"You can not end project already ended.",
               StatusCodes.Status409Conflict
           );
        }
        project.ActualEndDate = DateTime.UtcNow;
    }
    public async Task<TaskResponse> CreateProjectTaskAsync(Guid ProjectId, CreateTaskRequest taskRequest, Guid CurrentUserId)
    {
        var project = await dbContext.Projects.Include(p => p.Tasks).FirstOrDefaultAsync(p => p.Id == ProjectId);
        if (project is null)
            throw new BusinessRuleException(
                $"The Project with ProjectId = {ProjectId} Not Found",
                StatusCodes.Status404NotFound
            );

        if (CurrentUserId != project?.OwnerId)
            throw new BusinessRuleException(
               $"Only the Owner of project can add Tasks.",
               StatusCodes.Status403Forbidden
           );
        if (project.ActualEndDate.HasValue)
        {
            throw new BusinessRuleException(
               $"You can not add tasks to ended project.",
               StatusCodes.Status409Conflict
           );
        }
        var task = new ProjectTask
        {
            Id = Guid.NewGuid(),
            Title = taskRequest.Title,
            Description = taskRequest.Description,
            AssignedUserId = taskRequest.AssignedUserId,
            CreatedAt = DateTime.UtcNow,
            ProjectId = ProjectId,
            Status = ProjectTaskStatus.NotStarted,
        };
        dbContext.Add(task);
        await dbContext.SaveChangesAsync();
        return TaskResponse.FromModel(task);
    }
    public async Task<bool> DeleteProjectAsync(Guid ProjectId, Guid CurrentUserId)
    {
        var project = await dbContext.Projects.Include(p => p.Tasks).FirstOrDefaultAsync(p => p.Id == ProjectId);
        if (project is null)
            throw new BusinessRuleException(
                $"The Project with ProjectId = {ProjectId} Not Found",
                StatusCodes.Status404NotFound
            );

        if (CurrentUserId != project?.OwnerId)
            throw new BusinessRuleException(
               $"Only the Owner of project can delete it.",
               StatusCodes.Status403Forbidden
           );

        dbContext.Projects.Remove(project);

        int value = await dbContext.SaveChangesAsync();
        return value > 0;
    }
    public async Task<bool> DeleteProjectTaskAsync(Guid ProjectId, Guid TaskId, Guid CurrentUserId)
    {
        var project = await dbContext.Projects.Include(p => p.Tasks).FirstOrDefaultAsync(p => p.Id == ProjectId);
        if (project is null)
            throw new BusinessRuleException(
                $"The Project with ProjectId = {ProjectId} Not Found",
                StatusCodes.Status404NotFound
            );
        var task = project.Tasks.FirstOrDefault(t => t.Id == TaskId);

        if (task is null)
            throw new BusinessRuleException("Task with TaskID = {TaskId} Not Found.", StatusCodes.Status404NotFound);
        if (CurrentUserId != project?.OwnerId)
            throw new BusinessRuleException(
               $"Only the Owner of project can delete it.",
               StatusCodes.Status403Forbidden
           );

        project.Tasks.Remove(task);

        int value = await dbContext.SaveChangesAsync();
        return value > 0;
    }
    public async Task<bool> UpdateTaskStatusCodeAsync(
        Guid ProjectId,
        Guid TaskId,
        UpdateTaskStatusRequest StatusRequest,
        Guid CurrentUserId
    )
    {
        var project = await dbContext.Projects.Include(p => p.Tasks).FirstOrDefaultAsync(p => p.Id == ProjectId);

        if (project is null)
            throw new BusinessRuleException(
                $"The Project with ProjectId = {ProjectId} Not Found",
                StatusCodes.Status404NotFound
            );
        var task = project?.Tasks.FirstOrDefault(t => t.Id == TaskId);
        if (task is null)
            throw new BusinessRuleException(
                $"The Task with TaskId = {TaskId} Not Found",
                StatusCodes.Status404NotFound
            );
        if (project?.OwnerId != CurrentUserId && task?.AssignedUserId != CurrentUserId)
        {
            throw new BusinessRuleException($"Only the assigned user or the Project Owner can update the task.", StatusCodes.Status403Forbidden);
        }

        else
        {

            task.Status = StatusRequest.Status;
            int value = await dbContext.SaveChangesAsync();
            return value > 0;

        }
    }
    public async Task<bool> UpdateProjectBudgetAsync(Guid ProjectId, UpdateBudgetRequest updateBudgetRequest, Guid CurrentUserId)
    {
        var project = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == ProjectId);

        if (project is null)
            throw new BusinessRuleException(
                $"The Project with ProjectId = {ProjectId} Not Found",
                StatusCodes.Status404NotFound
            );
        else if (CurrentUserId != project.OwnerId)
            throw new BusinessRuleException(
               $"Only the Owner of project can updated it.",
               StatusCodes.Status403Forbidden
           );
        else
        {
            project.Budget = updateBudgetRequest.Budget;

            int value = await dbContext.SaveChangesAsync();
            return value > 0;
        }
    }
    public async Task AssignUserToTaskAsync(Guid ProjectId, Guid TaskId, Guid UserId, Guid CurrentUserId)
    {
        var project = await dbContext.Projects.Include(p => p.Tasks).FirstOrDefaultAsync(p => p.Id == ProjectId);
        if (project is null)
            throw new BusinessRuleException(
                $"The Project with ProjectId = {ProjectId} Not Found",
                StatusCodes.Status404NotFound
            );
        var task = project?.Tasks.FirstOrDefault(t => t.Id == TaskId);

        if (task is null)
            throw new BusinessRuleException(
                $"The Task with TaskId = {TaskId} Not Found",
                StatusCodes.Status404NotFound
            );
        if (project?.OwnerId != CurrentUserId && task?.AssignedUserId != CurrentUserId)
        {
            throw new BusinessRuleException($"Only the assigned user or the Project Owner can assign users to the task.", StatusCodes.Status409Conflict);
        }
        else
        {
            task.AssignedUserId = UserId;
            await dbContext.SaveChangesAsync();

        }
    }
}




