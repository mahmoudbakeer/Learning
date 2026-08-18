namespace LoggerCategory.Services;

public class ProcessService(ILogger<ProcessService> logger)
{
    public Task Process(Guid processId)
    {
        logger.LogTrace("Trace: Starting processing for ProcessId: {ProcessId}", processId);
        logger.LogDebug("Debugging information for ProcessId: {ProcessId}", processId);
        logger.LogInformation("Processing request for ProcessId: {ProcessId}", processId);
        logger.LogWarning("Warning: Potential issue detected for ProcessId: {ProcessId}", processId);
        logger.LogError("Error occurred while processing ProcessId: {ProcessId}", processId);
        logger.LogCritical("Critical error encountered for ProcessId: {ProcessId}", processId);
        // Simulate processing logic
        return Task.CompletedTask;
    }
}