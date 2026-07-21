namespace RestWithJWTSecurity.Permissions;

public static class Permission
{
    public static class Project
    {
        public const string Create = "project:create";
        public const string Read = "project:read";
        public const string Update = "project:update";
        public const string Delete = "project:delete";
        public const string AssignMember = "project:assign_member";
        public const string ManageBudget = "project:manage_budget";
    }

    public static class Task
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