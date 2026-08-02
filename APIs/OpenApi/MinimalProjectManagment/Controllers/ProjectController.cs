using System.Security.Claims;
using Asp.Versioning;
using Asp.Versioning.Conventions;
using ControllerProjectManagement.Entities;
using ControllerProjectManagement.Permission;
using ControllerProjectManagement.Requests;
using ControllerProjectManagement.Responses;
using ControllerProjectManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using SQLitePCL;

namespace ControllerProjectManagement.Controllers;

[ApiController]
[Authorize]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/projects")]
[Tags("Projects")]
public class ProjectController(IProjectServices projectServices) : ControllerBase
{

    // never make non-endpoint method public in Controller
    private Guid GetUserId() => Guid.Parse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    [HttpGet]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [EndpointName("GetProjectsV1")]
    [EndpointDescription("Get all projects.")]
    [EndpointSummary("Retrieve all the version 1 project responses.")]
    [Authorize(Policy = PermissionRoot.Project.Read)]
    public async Task<ActionResult<List<ProjectResponse>>> GetProjectsV1()
    {
        return Ok(await projectServices.GetProjectsAsync());
    }
    [HttpGet("{ProjectId:Guid}")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [EndpointName("GetProjectV1")]
    [EndpointDescription("Get project.")]
    [EndpointSummary("Retrieve the project has the mapped Id version 1.")]
    [Authorize(Policy = PermissionRoot.Project.Read)]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult<ProjectResponse>> GetProjectV1([FromRoute] Guid ProjectId)
    {
        var project = await projectServices.GetProjectAsync(ProjectId);
        return Ok(project);
    }
    [HttpGet]
    [Authorize(Policy = PermissionRoot.Project.Read)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [EndpointName("GetProjectsV2")]
    [EndpointDescription("Get all projects.")]
    [EndpointSummary("Retrieve all the version 2 project responses.")]
    [MapToApiVersion("2.0")]
    public async Task<ActionResult<List<ProjectResponse>>> GetProjectsV2()
    {
        var projects = await projectServices.GetProjectsAsync();

        foreach (var p in projects)
            p.Currency = "USD";
        return Ok(projects);
    }
    [HttpGet("{ProjectId:Guid}")]
    [Authorize(Policy = PermissionRoot.Project.Read)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [EndpointName("GetProjectV2")]
    [EndpointDescription("Get project.")]
    [EndpointSummary("Retrieve the project has the mapped Id version 2.")]
    [MapToApiVersion("2.0")]
    public async Task<ActionResult<ProjectResponse>> GetProjectV2([FromRoute] Guid ProjectId)
    {
        var project = await projectServices.GetProjectAsync(ProjectId);
        project.Currency = "USD";
        return Ok(project);
    }
    [HttpDelete("{ProjectId:Guid}")]
    [Authorize(Policy = PermissionRoot.Project.Delete)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [EndpointName("DeleteProjectV1")]
    [EndpointDescription("Delete Project.")]
    [EndpointSummary("Delete the project has the mapped Id version 1.")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> DeleteProjectV1([FromRoute] Guid ProjectId)
    {
        var UserId = GetUserId();
        await projectServices.DeleteProjectAsync(ProjectId, UserId);
        return NoContent();

    }
    [HttpPut("{ProjectId:Guid}")]
    [Authorize(Policy = PermissionRoot.Project.Update)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [EndpointName("UpdateProjectV1")]
    [EndpointDescription("Update Project.")]
    [EndpointSummary("Update the project has the mapped Id version 1.")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> UpdateProjectV1([FromRoute] Guid ProjectId, [FromBody] UpdateProjectRequest projectRequest)
    {
        await projectServices.UpdateProjectAsync(ProjectId, projectRequest, GetUserId());
        return NoContent();
    }
    [HttpPost]
    [Authorize(Policy = PermissionRoot.Project.Create)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [EndpointName("CreateProjectV1")]
    [EndpointDescription("Create new Project.")]
    [EndpointSummary("Create new project in version 1.")]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult<ProjectResponse>> CreateProjectV1([FromBody] CreateProjectRequest projectRequest)
    {
        var project = await projectServices.CreateProjectAsync(projectRequest, GetUserId());
        return CreatedAtAction(
            actionName: nameof(GetProjectV1),
            routeValues: new { ProjectId = project.Id },
            value: project
        );
    }
    [HttpPut("{ProjectId:Guid}/budget")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [EndpointName("UpdateBudgetV1")]
    [EndpointDescription("Update Project Budget.")]
    [EndpointSummary("Update the budget of the project in version 1.")]
    [Authorize(Policy = PermissionRoot.Project.ManageBudget)]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> UpdateBudgetV1([FromRoute] Guid ProjectId, [FromBody] UpdateBudgetRequest budgetRequest)
    {
        await projectServices.UpdateProjectBudgetAsync(ProjectId, budgetRequest, GetUserId());

        return NoContent();
    }
    [HttpPut("{ProjectId:Guid}/end")]
    [Authorize(Policy = PermissionRoot.Project.Update)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [EndpointName("EndProjectV1")]
    [EndpointDescription("End Project.")]
    [EndpointSummary("End the Progress of the project in version 1.")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> EndProjectV1([FromRoute] Guid ProjectId)
    {
        await projectServices.EndProjectAsync(ProjectId, GetUserId());
        return NoContent();
    }

    // Task EndPoints

    [HttpGet("{ProjectId:Guid}/Tasks/{TaskId:Guid}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [EndpointName("GetTaskV1")]
    [EndpointDescription("Get ProjectTask.")]
    [EndpointSummary("Retrieve the task with specified taskId of the Project with specified ProjectId.")]
    [Authorize(Policy = PermissionRoot.ProjectTask.Read)]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult<TaskResponse>> GetTaskV1([FromRoute] Guid ProjectId, [FromRoute] Guid TaskId)
    {
        var task = await projectServices.GetTaskAsync(ProjectId, TaskId);

        return Ok(task);
    }
    [HttpPost("{ProjectId:Guid}/Tasks")]
    [Authorize(Policy = PermissionRoot.ProjectTask.Create)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [EndpointName("CreateTaskV1")]
    [EndpointDescription("Create new ProjectTask.")]
    [EndpointSummary("Add new task to the Project with specified ProjectId.")]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult<TaskResponse>> CreateTaskV1([FromRoute] Guid ProjectId, [FromBody] CreateTaskRequest taskRequest)
    {
        var task = await projectServices.CreateProjectTaskAsync(ProjectId, taskRequest, GetUserId());

        return CreatedAtAction(
            actionName: nameof(GetTaskV1),
            routeValues: new { ProjectId = ProjectId, TaskId = task.Id },
            value: task
        );
    }
    [HttpPut("{ProjectId:Guid}/Tasks/{TaskId:Guid}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [EndpointName("UpdateTaskV1")]
    [EndpointDescription("Update ProjectTask.")]
    [EndpointSummary("Update ProjectTask with the Specified TaskId of the Project with specified ProjectId.")]
    [Authorize(Policy = PermissionRoot.ProjectTask.Update)]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> UpdateTaskV1([FromRoute] Guid ProjectId, [FromRoute] Guid TaskId, [FromBody] UpdateTaskRequest taskRequest)
    {
        await projectServices.UpdateTaskAsync(ProjectId, TaskId, taskRequest, GetUserId());

        return NoContent();
    }

    [HttpDelete("{ProjectId:Guid}/Tasks/{TaskId:Guid}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [EndpointName("DeleteTaskV1")]
    [EndpointDescription("Delete ProjectTask.")]
    [EndpointSummary("Delete ProjectTask with the Specified TaskId of the Project with specified ProjectId.")]
    [Authorize(Policy = PermissionRoot.ProjectTask.Delete)]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> DeleteTaskV1([FromRoute] Guid ProjectId, [FromRoute] Guid TaskId)
    {
        await projectServices.DeleteProjectTaskAsync(ProjectId, TaskId, GetUserId());
        return NoContent();
    }
    [HttpPut("{ProjectId:Guid}/Tasks/{TaskId:Guid}/Status")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [EndpointName("UpdateTaskStatusV1")]
    [EndpointDescription("Update ProjectTask Status.")]
    [EndpointSummary("Update ProjectTask Status with the Specified TaskId of the Project with specified ProjectId.")]
    [Authorize(Policy = PermissionRoot.ProjectTask.UpdateStatus)]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> UpdateTaskStatusV1([FromRoute] Guid ProjectId, [FromRoute] Guid TaskId, [FromBody] UpdateTaskStatusRequest taskStatusRequest)
    {
        await projectServices.UpdateTaskStatusCodeAsync(ProjectId, TaskId, taskStatusRequest, GetUserId());

        return NoContent();
    }

    [HttpPut("{ProjectId:Guid}/Tasks/{TaskId:Guid}/{UserId:Guid}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [EndpointName("AssignUserToTaskV1")]
    [EndpointDescription("AssignUser to ProjectTask.")]
    [EndpointSummary("Assign User to ProjectTask with the Specified TaskId of the Project with specified ProjectId.")]
    [Authorize(Policy = PermissionRoot.ProjectTask.AssignUser)]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> AssignUserToTaskV1([FromRoute] Guid ProjectId, [FromRoute] Guid TaskId, [FromRoute] Guid UserId)
    {
        await projectServices.AssignUserToTaskAsync(ProjectId, TaskId, UserId, GetUserId());
        return NoContent();
    }
}