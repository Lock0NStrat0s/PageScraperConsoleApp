using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageScraper.Models;

internal class Zillow_MLSModel
{
    public string Price { get; set; }
    public string PropertyAddress { get; set; }
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public int SquareFootage { get; set; }
    public int YearBuilt { get; set; }
    public string MLSNumber { get; set; }
    public string Additional { get; set; }
}
