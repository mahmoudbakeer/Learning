namespace LoggerCategory.Services;

public class ProcessService(ILogger<ProcessService> logger)
{
    public Task Process(Guid processId)
    {
        logger.LogInformation("Processing request for ProcessId: {ProcessId}", processId);
        // Simulate processing logic
        return Task.CompletedTask;
    }
}