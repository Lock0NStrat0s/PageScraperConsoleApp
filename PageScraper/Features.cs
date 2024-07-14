using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageScraper;

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
