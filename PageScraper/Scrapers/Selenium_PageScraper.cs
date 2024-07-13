using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageScraper.Scrapers;

internal class Selenium_PageScraper
{
    private string _url { get; set; }
    private HashSet<string> links = new HashSet<string>();                  // store all href URLS
    private HashSet<string> images = new HashSet<string>();                 // store all src URLs
    public Selenium_PageScraper(string url)
    {
        Console.WriteLine("Selenium Page Scraper");
        _url = url;
    }

    public async Task Start()
    {
        // create driver
        var options = new ChromeOptions();
        options.AddArguments(new List<string> { "--verbose", "headless", "disable-dev-shm-usage" });
        var driver = new ChromeDriver(options);

        try
        {
            // navigate to webpage
            driver.Navigate().GoToUrl(_url);

            Thread.Sleep(5000);

            Console.WriteLine($"TITLE: {driver.Title}");

            // get all links
            //var links = driver.FindElements(By.TagName("a")).Select(y => y.Text).Where(x => x.Length > 0).ToList();
            //links.ForEach(x => Console.WriteLine(x));

            // LINKS
            driver.FindElements(By.XPath("//a[@href]")).Select(y => y.GetAttribute("href")).Where(z => z.Length > 0).ToList().ForEach(x => links.Add(x));
            Console.WriteLine("LINKS:");
            links.ToList().ForEach(url => Console.WriteLine(url));

            // IMAGES
            driver.FindElements(By.XPath("//img[@src]")).Select(y => y.GetAttribute("src")).Where(z => z.Length > 0).ToList().ForEach(x => images.Add(x));
            Console.WriteLine("\n\nIMAGES:");
            images.ToList().ForEach(url => Console.WriteLine(url));

            // META
            Console.WriteLine("\n\nALL TEXT ON PAGE:");
            // Find all meta elements
            IList<IWebElement> metaElements = driver.FindElements(By.TagName("meta"));

            // Extract and print meta tag information
            foreach (IWebElement element in metaElements)
            {
                string name = element.GetAttribute("name");
                string content = element.GetAttribute("content");

                if (!string.IsNullOrEmpty(name))
                {
                    Console.WriteLine($"Name: {name}, Content: {content}");
                }
                else
                {
                    string property = element.GetAttribute("property");
                    if (!string.IsNullOrEmpty(property))
                    {
                        Console.WriteLine($"Property: {property}, Content: {content}");
                    }
                }
            }


            // ALL TEXT
            // Locate the body element
            IWebElement bodyElement = driver.FindElement(By.TagName("body"));

            // Extract and print the text from the body element
            string pageText = bodyElement.Text;
            Console.WriteLine("\n\nPage Text:");
            Console.WriteLine(pageText);

        }
        catch (NoSuchElementException e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            driver.Quit();
        }
    }
}
