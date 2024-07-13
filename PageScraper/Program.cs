using PageScraper.Scrapers;

namespace PageScraper;

public class Program
{
    static async Task Main(string[] args)
    {
        //HTTP_PageScraper scraper = new HTTP_PageScraper();
        //await scraper.Scrape("https://www.markaz.ca/");


        //Selenium_PageScraper seleniumScraper = new Selenium_PageScraper("https://markaz.ca/");
        //YouTubeScraper ytScraper = new YouTubeScraper("https://youtu.be/sTXqHi0lOX8?si=PCcvnAmbVglGWLzx");
        //await ytScraper.Start();

        ZillowScraper zScraper = new ZillowScraper("https://www.zillow.com/homedetails/10011-116th-St-NW-1002-Edmonton-AB-T5K-1V4/352574402_zpid/");
        //ZillowScraper zScraper = new ZillowScraper("https://www.zillow.com/homedetails/5005-31st-Ave-NW-408-Edmonton-AB-T6L-6S6/351930702_zpid/");
        await zScraper.Start();
    }
}
