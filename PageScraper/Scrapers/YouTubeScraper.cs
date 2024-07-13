using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;

namespace PageScraper.Scrapers;

class YouTubeScraper
{
    private string _url { get; set; }
    private HashSet<string> links = new HashSet<string>();                  // store all href URLS
    private HashSet<string> images = new HashSet<string>();                 // store all src URLs

    public YouTubeScraper(string url)
    {
        Console.WriteLine("Selenium Page Scraper");
        _url = url;
    }

    public async Task Start()
    {
    // Initialize the Chrome WebDriver
    IWebDriver driver = new ChromeDriver();

    try
    {
        // Navigate to a YouTube video page
        driver.Navigate().GoToUrl(_url);

        // Wait for the page to load and the title element to be present
        var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(10));
        wait.Until(d => d.FindElement(By.CssSelector("h1.title")));

        // Extract video title
        var titleElement = driver.FindElement(By.CssSelector("h1.title"));
        string videoTitle = titleElement.Text;
        Console.WriteLine("Title: " + videoTitle);

        // Extract video views
        var viewsElement = driver.FindElement(By.CssSelector("span.view-count"));
        string videoViews = viewsElement.Text;
        Console.WriteLine("Views: " + videoViews);

        // Extract video description
        var showMoreButton = driver.FindElement(By.CssSelector("yt-formatted-string.more-button"));
        showMoreButton.Click(); // Click the "Show More" button to reveal the full description

        var descriptionElement = driver.FindElement(By.CssSelector("yt-formatted-string.content"));
        string videoDescription = descriptionElement.Text;
        Console.WriteLine("Description: " + videoDescription);

        // Extract comments (example of extracting the first few comments)
        IList<IWebElement> commentElements = driver.FindElements(By.CssSelector("#comments #content-text"));
        foreach (var comment in commentElements)
        {
            Console.WriteLine("Comment: " + comment.Text);
        }
    }
    finally
    {
        // Close the browser
        driver.Quit();
    }
}
}
