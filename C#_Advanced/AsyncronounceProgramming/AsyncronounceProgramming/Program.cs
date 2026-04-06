using System.Net;

namespace AsyncronounceProgramming
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            Task task1 = DownloadAndPrintAsync("https://www.cnn.com");
            Console.WriteLine($"Task one started ...");
            Task task2 = DownloadAndPrintAsync("https://www.amazon.com");
            Console.WriteLine($"Task two started ...");
            Task task3 = DownloadAndPrintAsync("https://www.bbc.com");
            Console.WriteLine($"Task three started ...");

            // wait until all task finishs 
            await Task.WhenAll(task1, task2, task3);


            Console.WriteLine("All tasks are completed successfully....");
        }

        static async Task DownloadAndPrintAsync(string url)
        {

            string content;

            // Using statement ensures that the WebClient is disposed of properly
            using (WebClient client = new WebClient())
            {
                // Simulate some work by adding a delay
                await Task.Delay(100);

                // Download the content of the web page asynchronously
                content = await client.DownloadStringTaskAsync(url);
            }

            // Print the URL and the length of the downloaded content
            Console.WriteLine($"{url}: {content.Length} characters downloaded");
        }
    }
}
