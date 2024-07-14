using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageScraper.Scrapers;

internal class HTTP_PageScraper
{
    private static readonly HttpClient client = new HttpClient();           // client to make HTTP requests
    //private HashSet<string> links = new HashSet<string>();                  // set to store links
    //private HashSet<string> images = new HashSet<string>();                 // set to store image URLs

    /// <summary>
    /// Method to scrape the given URL
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public async Task Scrape(string url)
    {
        HttpResponseMessage response = await GetStatusCodeOfResponse(url);
        string html = await FetchHTMLContent(response);
        HtmlNodeCollection links = LoadHTMLContent(html);

        //int i = 0;
        //foreach (var item in links)
        //{
        //    //string result = item.InnerText == "" ? item.GetAttributeValue("href", "") : item.InnerText;
        //    Console.WriteLine($"{i++}: {item}");
        //}
    }

    /// <summary>
    /// Method to get the status code of the response
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    private static async Task<HttpResponseMessage> GetStatusCodeOfResponse(string url)
    {
        try
        {
            return await client.GetAsync(url);  // getasync is used to obtain the status code
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request failed: {e.Message}");
        }

        return null;
    }

    /// <summary>
    /// Method to fetch the HTML content of the response
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    private async Task<string> FetchHTMLContent(HttpResponseMessage response)
    {
        try
        {
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();   // read the content of the response
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request failed: {e.Message}");
        }

        return null;
    }

    /// <summary>
    /// Method to load the HTML content of the response
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    private static HtmlNodeCollection LoadHTMLContent(string html)
    {
        HtmlDocument document = new HtmlDocument();
        document.LoadHtml(html);

        HtmlNodeCollection links = document.DocumentNode.SelectNodes("//div[@data-testid]");  // select all div tags with the specified attribute
        return links;
    }
}
