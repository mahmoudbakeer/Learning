namespace ControllerProjectManagement.Permission
{
    public static class PermissionRoot
    {
        public static class Project
        {
            public const string Create = "project:create";
            public const string Read = "project:read";
            public const string Update = "project:update";
            public const string Delete = "project:delete";
            public const string ManageBudget = "project:manage_budget";
        }

        public static class ProjectTask
        {
            public const string Create = "task:create";
            public const string Read = "task:read";
            public const string Update = "task:update";
            public const string Delete = "task:delete";
            public const string AssignUser = "task:assign_user";
            public const string UpdateStatus = "task:update_status";
            public const string Comment = "task:comment";
        }
    }
}

