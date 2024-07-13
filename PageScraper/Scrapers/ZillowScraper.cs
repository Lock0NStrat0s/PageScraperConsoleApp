using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace PageScraper.Scrapers;

internal class ZillowScraper
{
    private readonly string _url;
    public ZillowScraper(string url)
    {
        _url = url;
    }

    public async Task Start()
    {
        // Initialize the Chrome WebDriver
        IWebDriver driver = new ChromeDriver();

        try
        {
            // Navigate to a Zillow listing page
            driver.Navigate().GoToUrl(_url);

            // Allow some time for the page to load
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Extract property address
            string address = driver.FindElement(By.XPath("//h1")).Text;

            // Extract price
            string price = driver.FindElement(By.XPath("//span[@data-testid='price']")).Text;

            // Extract number of bedrooms, bathrooms, and square footage
            List<string> details = driver.FindElements(By.XPath("//div[@data-testid='bed-bath-sqft-fact-container']")).Select(x => x.Text).ToList();
            string bedrooms = details[0].Split("\r\n")[0];
            string bathrooms = details[1].Split("\r\n")[0];
            string squareFootage = details[2].Split("\r\n")[0];

            // Extract year built and MLS number
            string yearBuilt = driver.FindElements(By.XPath("//span[@class='Text-c11n-8-100-2__sc-aiai24-0 sc-fSKiAx bSfDch cvvIFA']")).Select(x => x.Text).ToArray()[1];
            string mls = driver.FindElement(By.XPath("//div[@data-testid='listing-attribution-overview']")).Text;

            // Extract insight tags
            List<string> insights = driver.FindElements(By.XPath("//div[@aria-label='insights tags']")).Select(x => x.Text).ToList();

            // Extract additional features and description
            string description = driver.FindElement(By.XPath("//div[@data-testid='description']")).Text;

            var allData = driver.FindElements(By.XPath("//div[@data-testid]")).ToList();

            // Extract facts and features
            Dictionary<string, List<string>> dictFeatures = new Dictionary<string, List<string>>();
            var categories = driver.FindElements(By.XPath("//div[@data-testid='category-group']")).ToList();
            Console.WriteLine(categories[1]);
            //for (int i = 0; i < categories.Count(); i++)
            //{
            //    List<string> features = categories[i].Text.Split("\r\n").ToList();
            //    dictFeatures[features[0]] = features.Skip(1).ToList();
            //}

            //    // Store the extracted data
            //    Dictionary<string, string> propertyData = new Dictionary<string, string>
            //{
            //    { "Address", address },
            //    { "Price", price },
            //    { "Bedrooms", beds },
            //    { "Bathrooms", baths },
            //    { "Square Footage", sqft },
            //    { "Year Built", yearBuilt },
            //    { "MLS Number", mls },
            //    { "Features", features },
            //    { "Description", description }
            //};

            //    // Print the extracted data
            //    foreach (var entry in propertyData)
            //    {
            //        Console.WriteLine($"{entry.Key}: {entry.Value}");
            //    }
        }
        finally
        {
            // Close the browser
            driver.Quit();
        }
    }
}
