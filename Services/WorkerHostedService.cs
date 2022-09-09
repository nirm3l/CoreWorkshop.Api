using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Collections.Concurrent ;


public class WorkerHostedService : BackgroundService
{
    private bool loop = true;

    private static string URL = "https://stream.wikimedia.org/v2/stream/recentchange";
    
    private static FixedSizedQueue<WikiRecord> WIKI_RECORDS = new FixedSizedQueue<WikiRecord>{Limit=100};
    protected override async Task ExecuteAsync(CancellationToken stopToken) {
        while (!stopToken.IsCancellationRequested) {
            await LoadWikiRecords();
        }
    }

    private async Task LoadWikiRecords() {
        HttpClient client = new HttpClient();

        while (loop) {
            try {
                using (var streamReader = new StreamReader(await client.GetStreamAsync(URL))) {
                    while (!streamReader.EndOfStream) {
                        var message = await streamReader.ReadLineAsync();

                        if (message != null && message.StartsWith("data: ")) {
                            WIKI_RECORDS.Enqueue(JsonSerializer.Deserialize<WikiRecord>(message.Substring(6)));
                        }
                    }
                }
            } 
            catch (Exception ex) {
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
	    }
    }
    public static ConcurrentQueue<WikiRecord> GetWikiRecords() {
        return WIKI_RECORDS.GetQueue();
    }
}
