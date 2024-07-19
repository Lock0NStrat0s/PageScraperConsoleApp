using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using PageScraper.Models;
using SeleniumExtras.WaitHelpers;

namespace PageScraper.Scrapers;

public class ZillowScraper
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
            string[] fullAddress = driver.FindElement(By.XPath("//h1")).Text.Split(",");

            // Break address into components
            string street = fullAddress[0].Trim();
            string city = fullAddress[1].Trim();
            string divisionCode = fullAddress[2].Trim();
            string[] divCodeArray = divisionCode.Split(" ");
            string subdivision = divCodeArray[0];
            string code = divCodeArray[1];
            string country = char.IsLetter(code[0]) ? "Canada" : "US";
            Address PropertyAddress = new Address()
            {
                StreetAddress = street,
                City = city,
                StateOrProvince = subdivision,
                ZipOrPostalCode = code,
                Country = country
            };

            // Extract price and initialize year built variable
            string priceText = driver.FindElement(By.XPath("//span[@data-testid='price']")).Text;
            decimal price = ExtractValue<decimal>(priceText);
            int yearBuilt = 0;

            // Extract number of bedrooms, bathrooms, and square footage
            List<string> details = driver.FindElements(By.XPath("//div[@data-testid='bed-bath-sqft-fact-container']")).Select(x => x.Text).ToList();
            string bedroomsText = details[0].Split("\r\n")[0].Trim();
            int bedrooms = ExtractValue<int>(bedroomsText);

            string bathroomsText = details[1].Split("\r\n")[0].Trim();
            int bathrooms = ExtractValue<int>(bathroomsText);

            string squareFootageText = details[2].Split("\r\n")[0].Trim();
            decimal squareFootage = ExtractValue<decimal>(squareFootageText);

            // Extract MLS number and details of agents
            List<string> agentDetails = driver.FindElement(By.XPath("//div[@data-testid='listing-attribution-overview']")).Text.Split("\r\n").ToList();
            string lastChecked = agentDetails[0];
            string lastUpdated = agentDetails[1];

            List<Agent> agents = new List<Agent>();
            for (int i = 3; i < agentDetails.Count() - 1; i += 2)
            {
                string[] temp1 = agentDetails[i].Split(" ");
                string temp2 = agentDetails[i + 1];
                Agent agent = new Agent()
                {
                    Name = $"{temp1[0]} {temp1[1]}",
                    Phone = temp1[2],
                    Company = temp2
                };
                agents.Add(agent);
            }

            string mlsText = agentDetails[^1].Split("MLS#:").Last();
            int mls = ExtractValue<int>(mlsText);

            // Extract insight tags
            List<string> insights = driver.FindElement(By.XPath("//div[@aria-label='insights tags']")).Text.Split("\r\n").ToList();

            // Extract additional features and description
            string description = driver.FindElement(By.XPath("//div[@data-testid='description']")).Text;

            // Main features
            var h3 = driver.FindElements(By.XPath("//h3"));
            // Elements inside features
            var elements = driver.FindElements(By.XPath("//div[@data-testid='fact-category']"));

            // Create Features Liste
            List<FeatureInfo> mainFeaturesList = new List<FeatureInfo>();
            int start = 0;
            // Extract text from each element using JavaScript
            foreach (IWebElement h in h3)
            {
                string text = JSExecution(driver, h).Trim();
                string mainFeatureText = "";

                List<SubFeatureInfo> subFeaturesList = new List<SubFeatureInfo>();

                if (text != null || text != "")
                {
                    mainFeatureText = text;
                }

                bool isFinished = false;
                do
                {
                    string[] texts = null;
                    try
                    {
                        texts = JSExecution(driver, elements[start++]).Trim().Split("\r\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        isFinished = true;
                        break;
                    }
                    bool isFirst = true;

                    string subFeatureText = "";
                    List<string> subElements = new();

                    try
                    {
                        var nextItem = elements[start].Text.Trim().Split("\r\n").First();
                        if (nextItem == "Parking" || nextItem == "Type & style" || nextItem == "Community")
                        {
                            isFinished = true;
                        }
                        else if (nextItem == "Condition" || nextItem == "Location")
                        {
                            isFinished = true;
                            isFirst = false;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    foreach (string t in texts)
                    {
                        if (t != null || t != "")
                        {
                            if (isFirst)
                            {
                                subFeatureText = t;
                                isFirst = false;
                            }
                            else
                            {
                                subElements.Add(t);
                                if (t.Contains("Year built"))
                                {
                                    string yearBuiltText = t;
                                    yearBuilt = ExtractValue<int>(yearBuiltText);
                                }
                            }
                        }

                    }
                    subFeaturesList.Add(new SubFeatureInfo(subFeatureText, new List<string>(subElements)));
                } while (!isFinished);
                // add to feature class

                mainFeaturesList.Add(new FeatureInfo(mainFeatureText, new List<SubFeatureInfo>(subFeaturesList)));
                subFeaturesList.Clear();
            }

            PropertyModel propertyModel = new PropertyModel()
            {
                Price = price,
                PropertyAddress = PropertyAddress,
                Bedrooms = bedrooms,
                Bathrooms = bathrooms,
                SquareFootage = squareFootage,
                YearBuilt = yearBuilt,
                Insights = insights,
                Agents = agents,
                MLSNumber = mls,
                Features = mainFeaturesList,
                LastChecked = lastChecked,
                LastUpdated = lastUpdated,
                DateFound = DateTime.Now
            };
        }
        finally
        {
            // Close the browser
            driver.Quit();
        }
    }

    private T ExtractValue<T>(string text) where T : struct, IConvertible
    {
        StringBuilder extractor = new StringBuilder();
        foreach (var c in text)
        {
            if (char.IsDigit(c) || c == '.')
            {
                extractor.Append(c);
            }
        }
        string result = extractor.ToString();
        if (typeof(T) == typeof(decimal))
        {
            decimal.TryParse(result, out decimal value);
            object boxedValue = value;
            return (T)boxedValue;
        }
        else if (typeof(T) == typeof(int))
        {
            int.TryParse(result, out int value);
            object boxedValue = value;
            return (T)boxedValue;
        }

        return default;
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
