namespace BackgroundServices;

public class BlobStorageCleanUpBackgroundServices(
    ILogger<BlobStorageCleanUpBackgroundServices> logger
) : BackgroundService
{
    private readonly TimeSpan _Intervals = TimeSpan.FromSeconds(5);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            logger.LogInformation("CleanUp service started at time : {Time}.", DateTime.UtcNow);
            var periodectimer = new PeriodicTimer(_Intervals);

            while (await periodectimer.WaitForNextTickAsync(stoppingToken))
            {
                int orphens = Random.Shared.Next(1, 10);

                await Task.Delay(1000);

                logger.LogInformation(
                    "The CleanUpService cleaned {orphens} Orphened Services.",
                    orphens
                );
            }
            logger.LogInformation("CleanUp service ended at time : {Time}.", DateTime.UtcNow);
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "UnKnown Error occured in BlobStorageCleanUpBackgroundServices class."
            );
        }
    }
}
