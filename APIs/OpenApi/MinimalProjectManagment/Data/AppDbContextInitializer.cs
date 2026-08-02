using ControllerProjectManagement.Entities;

namespace ControllerProjectManagement.Data;

public static class AppDbContextInitializer
{
    public static async Task InitializeAsync(AppDbContext context)
    {
        var fakeUser1Id = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var fakeUser2Id = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var fakeUser3Id = Guid.Parse("33333333-3333-3333-3333-333333333333");

        await context.Database.EnsureCreatedAsync();

        if (context.Projects.Any()) return;

        var ecommerce = new Project
        {
            Id = Guid.NewGuid(),
            Name = "E-Commerce Platform",
            Description = "Online shopping system with cart, checkout, and payment.",
            ExpectedStartDate = DateTime.UtcNow,
            OwnerId = fakeUser1Id,
            CreatedAt = DateTime.UtcNow,
            Tasks = new List<ProjectTask>
            {
                new() { Id = Guid.NewGuid(), AssignedUserId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Title = "Design product catalog", CreatedAt = DateTime.UtcNow  },
                new() { Id = Guid.NewGuid(), AssignedUserId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Title = "Implement shopping cart", CreatedAt = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), AssignedUserId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Title = "Add payment gateway", CreatedAt = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), AssignedUserId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Title = "Set up user authentication", CreatedAt = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), AssignedUserId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Title = "Deploy initial version", CreatedAt = DateTime.UtcNow }
            }
        };

        context.Projects.AddRange(ecommerce);

        await context.SaveChangesAsync();
    }
}