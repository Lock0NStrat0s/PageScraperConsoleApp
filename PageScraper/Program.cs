using PageScraper.Scrapers;

namespace PageScraper;

public class Program
{
    static async Task Main(string[] args)
    {
        //HTTP_PageScraper scraper = new HTTP_PageScraper();
        //await scraper.Scrape("https://www.zillow.com/homedetails/6718-36a-Ave-NW-Edmonton-AB-T6K-1S3/2055462236_zpid/");


        //Selenium_PageScraper seleniumScraper = new Selenium_PageScraper("https://markaz.ca/");
        //YouTubeScraper ytScraper = new YouTubeScraper("https://youtu.be/sTXqHi0lOX8?si=PCcvnAmbVglGWLzx");
        //await ytScraper.Start();

        //ZillowScraper zScraper = new ZillowScraper("https://www.zillow.com/homedetails/10011-116th-St-NW-1002-Edmonton-AB-T5K-1V4/352574402_zpid/");
        //ZillowScraper zScraper = new ZillowScraper("https://www.zillow.com/homedetails/1209-Magnolia-Pl-Birmingham-AL-35215/897903_zpid/");
        ZillowScraper zScraper = new ZillowScraper("https://www.zillow.com/homedetails/176-Redstone-Way-Birmingham-AL-35215/906905_zpid/");
        await zScraper.Start();
    }
}
