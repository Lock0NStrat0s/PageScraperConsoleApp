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
    public string PropertyAddress { get; set; }
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public decimal SquareFootage { get; set; }
    public int YearBuilt { get; set; }
    public List<string> AgentDetails { get; set; }
    public int MLSNumber { get; set; }
    //public SubFeatureInfo SubFeatures { get; set; }
    //public FeatureInfo MainFeatures { get; set; }
    public FeaturesCollection Features { get; set; }
    public DateTime DateCreated { get; set; }
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

public class FeaturesCollection
{
    public List<FeatureInfo> Features { get; set; }

    public FeaturesCollection(List<FeatureInfo> features)
    {
        Features = features;
    }
}