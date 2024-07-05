namespace PageScraper;

public class Program
{
    static async Task Main(string[] args)
    {
        HTTP_PageScraper scraper = new HTTP_PageScraper();
        await scraper.Scrape("https://www.markaz.ca/");
    }
}
