using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent ;

namespace CoreWorkshop.Api.Controllers;

[ApiController]
[Route("")]
public class TitlesController : ControllerBase
{
    [HttpGet(Name = "Titles")]
    [Route("/titles")]
    public async Task GetTitles()
    {
        setupResponse();

        WikiRecord lastRecord = null;
            
        while (true) {
            var records = WikiService.GetNewWikiRecords(lastRecord);

            foreach (WikiRecord record in records) {
                await Response.WriteAsync($"data: {record.title}\r\r");

                lastRecord = record;
            }

            await Task.Delay(100);
        }
    }

    [HttpGet(Name = "WikiTitles")]
    [Route("/titles/{wiki}")]
    public async Task GetWikiTitles(string wiki)
    {
        setupResponse();

        WikiRecord lastRecord = null;
            
        while (true) {
            var records = WikiService.GetNewWikiRecords(lastRecord);

            foreach (WikiRecord record in records) {
                if (record.wiki.Equals(wiki)) {
                    await Response.WriteAsync($"data: {record.title}\r\r");
                }

                lastRecord = record;
            }

            await Task.Delay(100);
        }
    }

    private void setupResponse() {
        Response.Headers.Add("Cache-Control", "no-cache");
        Response.Headers.Add("Content-Type", "text/event-stream");
        Response.Headers.Add("Connection", "keep-alive");
    }
}
