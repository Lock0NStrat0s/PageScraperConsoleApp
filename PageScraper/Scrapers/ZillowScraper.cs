using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using static System.Net.Mime.MediaTypeNames;

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
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@data-testid]")));

            // Extract property address
            string address = driver.FindElement(By.XPath("//h1")).Text;

            // Extract price
            string priceText = driver.FindElement(By.XPath("//span[@data-testid='price']")).Text;
            StringBuilder moneyExtractor = new StringBuilder();
            foreach (var c in priceText)
            {
                if (char.IsDigit(c) || c == '.')
                {
                    moneyExtractor.Append(c);
                }
            }
            string moneyString = moneyExtractor.ToString();
            decimal.TryParse(moneyString, out decimal price);

            // Extract number of bedrooms, bathrooms, and square footage
            List<string> details = driver.FindElements(By.XPath("//div[@data-testid='bed-bath-sqft-fact-container']")).Select(x => x.Text).ToList();
            string bedrooms = details[0].Split("\r\n")[0];
            string bathrooms = details[1].Split("\r\n")[0];
            string squareFootage = details[2].Split("\r\n")[0];

            // Extract MLS number
            string[] mls = driver.FindElement(By.XPath("//div[@data-testid='listing-attribution-overview']")).Text.Split("\r\n");

            // Extract insight tags
            string s = driver.FindElement(By.XPath("//h2")).Text;
            List<string> i = driver.FindElements(By.XPath("//div[@aria-label='insights tags']")).Select(x => x.Text).ToList();
            (string special, List<string> insights) specialInsights = (s, i);

            // Extract additional features and description
            string description = driver.FindElement(By.XPath("//div[@data-testid='description']")).Text;

            // Main features
            var h3 = driver.FindElements(By.XPath("//h3"));
            // Elements inside features
            var elements = driver.FindElements(By.XPath("//div[@data-testid='fact-category']"));

            List<string> mainFeatures = new();
            List<string> subFeatures = new();
            List<string> subElements = new();

            // Extract text from each element using JavaScript
            foreach (IWebElement item in h3)
            {
                string text = JSExecution(driver, item).Trim();
                if (text != null || text != "")
                {
                    mainFeatures.Add(text);
                }
            }

            foreach (IWebElement item in elements)
            {
                string[] text = JSExecution(driver, item).Trim().Split("\r\n");
                bool isFirst = true;

                foreach (var element in text)
                {
                    if (element != null || element != "")
                    {
                        if (isFirst)
                        {
                            subFeatures.Add(element);
                            isFirst = false;
                        }
                        else
                        {
                            subElements.Add(element);
                        }
                    }
                }
            }
        }
        finally
        {
            // Close the browser
            driver.Quit();
        }
    }

    private string JSExecution(IWebDriver driver, IWebElement element)
    {
        // Scroll the element into view
        try
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
            // Extract the text using JavaScript to avoid truncation issues
            return (string)((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].innerText;", element);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}
