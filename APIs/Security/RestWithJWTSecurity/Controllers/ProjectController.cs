// namespace RestWithJWTSecurity.Controllers;
//
// using System.Security.Claims;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
//
// [ApiController]
// [Route("api/projects")]
// [Authorize] // Apply Authorize at the controller level to ensure all actions require authentication by default
// public class ProjectsController : ControllerBase
// {
//     [HttpGet]
//     [Authorize(Policy = Permission.Project.Read)] // Requires 'project:read' permission
//     public IActionResult GetProjects()
//     {
//         return Ok(
//             new
//             {
//                 Message = "Read List of project",
//                 UserInfo = GetUserInfo(),
//                 Permission = Permission.Project.Read, // Using constant for consistency
//             }
//         );
//     }
//
//     [HttpGet("{id}")]
//     [Authorize(Policy = Permission.Project.Read)] // Requires 'project:read' permission
//     public IActionResult GetProjectById(string id)
//     {
//         return Ok(
//             new
//             {
//                 Message = $"Read a single project with id = `{id}`",
//                 UserInfo = GetUserInfo(),
//                 Permission = Permission.Project.Read, // Using constant for consistency
//             }
//         );
//     }
//
//     [HttpPost]
//     [Authorize(Policy = Permission.Project.Create)] // Requires 'project:create' permission
//     public IActionResult CreateProject()
//     {
//         return CreatedAtAction(
//             nameof(GetProjectById),
//             new { id = Guid.NewGuid() },
//             new
//             {
//                 Message = "Project created successfully",
//                 UserInfo = GetUserInfo(),
//                 Permission = Permission.Project.Create, // Using constant for consistency
//             }
//         );
//     }
//
//     [HttpPut("{id}")]
//     [Authorize(Policy = Permission.Project.Update)] // Requires 'project:update' permission
//     public IActionResult UpdateProject(string id)
//     {
//         return Ok(
//             new
//             {
//                 Message = $"Project with Id = '{id}' was updated successfully",
//                 UserInfo = GetUserInfo(),
//                 Permission = Permission.Project.Update, // Using constant for consistency
//             }
//         );
//     }
//
//     [HttpDelete("{id}")]
//     [Authorize(Policy = Permission.Project.Delete)] // Requires 'project:delete' permission
//     public IActionResult DeleteProject(string id)
//     {
//         var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//
//         return Ok(
//             new
//             {
//                 Message = $"Project with Id = '{id}' was Deleted successfully",
//                 UserInfo = GetUserInfo(),
//                 Permission = "Project:Read", // Using constant for consistency
//             }
//         );
//     }
//
//     [HttpPost("{id}/members")]
//     [Authorize(Policy = Permission.Project.AssignMember)] // Requires 'project:assign_member' permission
//     public IActionResult AssignMember(string id)
//     {
//         return CreatedAtAction(
//             nameof(GetProjectById),
//             new { id },
//             new
//             {
//                 Message = $"A memeber has been assigned successfully to project '{id}'",
//                 UserInfo = GetUserInfo(),
//                 Permission = "Project:Create", // Using constant for consistency
//             }
//         );
//     }
//
//     [HttpGet("{id}/budget")]
//     [Authorize(Policy = Permission.Project.ManageBudget)] // Requires 'project:manage_budget' permission
//     public IActionResult GetProjectBudget(string id)
//     {
//         return Ok(
//             new
//             {
//                 Message = $"Successfully accessed the budget for project '{id}'",
//                 UserInfo = GetUserInfo(),
//                 Permission = "Project:manage_budget", // Using constant for consistency
//             }
//         );
//     }
//
//     [HttpGet("tasks/{taskId}")]
//     [Authorize(Policy = Permission.Task.Read)] // Requires 'task:read' permission
//     public IActionResult GetTask(string taskId)
//     {
//         return Ok(
//             new
//             {
//                 Message = $"Task '{taskId}' details retrieved",
//                 UserInfo = GetUserInfo(),
//                 Permission = "Task:Read", // Using constant for consistency
//             }
//         );
//     }
//
//     [HttpPost("tasks")]
//     [Authorize(Policy = Permission.Task.Create)] // Requires 'task:create' permission
//     public IActionResult CreateTask()
//     {
//         var taskId = Guid.NewGuid().ToString();
//
//         return Created(
//             $"/api/projects/tasks/{taskId}",
//             new
//             {
//                 Message = $"Task '{taskId}' created successfully",
//                 UserInfo = GetUserInfo(),
//                 Permission = "Task:Create", // Using constant for consistency
//             }
//         );
//     }
//
//     [HttpPost("tasks/{taskId}/assign")]
//     [Authorize(Policy = Permission.Task.AssignUser)] // Requires 'task:assign_user' permission
//     public IActionResult AssignUserToTask(string taskId)
//     {
//         return Ok(
//             new
//             {
//                 Message = $"User assigned to task '{taskId}' successfully",
//                 UserInfo = GetUserInfo(),
//                 Permission = "Task:AssignUser", // Using constant for consistency
//             }
//         );
//     }
//
//     [HttpPut("tasks/{taskId}")]
//     [Authorize(Policy = Permission.Task.Update)] // Requires 'task:update' permission
//     public IActionResult UpdateTask(string taskId)
//     {
//         return Ok(
//             new
//             {
//                 Message = $"Task '{taskId}' updated successfully",
//                 UserInfo = GetUserInfo(),
//                 Permission = "Task:UpdateTask", // Using constant for consistency
//             }
//         );
//     }
//
//     [HttpDelete("tasks/{taskId}")]
//     [Authorize(Policy = Permission.Task.Delete)] // Requires 'task:delete' permission
//     public IActionResult DeleteTask(string taskId)
//     {
//         return Ok(
//             new
//             {
//                 Message = $"Task '{taskId}' deleted successfully",
//                 UserInfo = GetUserInfo(),
//                 Permission = "Task:DeleteTask", // Using constant for consistency
//             }
//         );
//     }
//
//     [HttpPut("tasks/{taskId}/status")]
//     [Authorize(Policy = Permission.Task.UpdateStatus)] // Requires 'task:update_status' permission
//     public IActionResult UpdateTaskStatus(string taskId)
//     {
//         return Ok(
//             new
//             {
//                 Message = $"Successfully updated status for task '{taskId}'",
//                 UserInfo = GetUserInfo(),
//                 Permission = "Task:UpdateStatus", // Using constant for consistency
//             }
//         );
//     }
//
//     // Helper method to extract user information from the claims principal
//     private string GetUserInfo()
//     {
//         if (User.Identity is { IsAuthenticated: false })
//             return "Anonymous"; // Corrected typo: Annonymous -> Anonymous
//
//         var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//         var firstName = User.FindFirstValue(ClaimTypes.GivenName);
//         var lastName = User.FindFirstValue(ClaimTypes.Surname);
//
//         return $"[{userId}] {firstName} {lastName}";
//     }
// }
