using OpenQA.Selenium.DevTools.V124.Autofill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageScraper.Models;

public class PropertyModel
{
    public int PropertyID { get; set; }
    public decimal Price { get; set; }
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public decimal SquareFootage { get; set; }
    public int YearBuilt { get; set; }
    public Address PropertyAddress { get; set; }
    public List<Agent> Agents { get; set; }
    public List<string> Insights { get; set; }
    public int MLSNumber { get; set; }
    public List<FeatureInfo> Features { get; set; }
    public string LastChecked { get; set; }
    public string LastUpdated { get; set; }
    public DateTime DateFound { get; set; }
}

public class Address
{
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string StateOrProvince { get; set; }
    public string ZipOrPostalCode { get; set; }
    public string Country { get; set; }
}

public class Agent
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Company { get; set; }
}

public class SubFeatureInfo
{
    public string SubFeature { get; set; }
    public List<string> SubElements { get; set; }

    public SubFeatureInfo(string subFeature, List<string> subElements)
    {
        SubFeature = subFeature;
        SubElements = subElements;
    }
}

public class FeatureInfo
{
    public string MainFeatures { get; set; }
    public List<SubFeatureInfo> SubFeatures { get; set; }

    public FeatureInfo(string mainFeatures, List<SubFeatureInfo> subFeatures)
    {
        MainFeatures = mainFeatures;
        SubFeatures = subFeatures;
    }
}