public class WikiService {

    public static ICollection<WikiRecord> GetNewWikiRecords(WikiRecord lastWikiRecord) {

        var wikiRecords = new LinkedList<WikiRecord>();

        if (WorkerHostedService.GetWikiRecords().Last() != lastWikiRecord) {
            WikiRecord[] allRecords = WorkerHostedService.GetWikiRecords().ToArray();
            
            foreach (var wikiRecord in allRecords.Reverse()) {
                if (wikiRecord == lastWikiRecord) {
                    break;
                }

                wikiRecords.AddFirst(wikiRecord);
            }
        }

        return wikiRecords;
    }
}
