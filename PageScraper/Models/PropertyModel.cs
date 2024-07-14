using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageScraper.Models;

internal class PropertyModel
{
    public int PropertyID { get; set; }
    public decimal Price { get; set; }
    public string PropertyAddress { get; set; }
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public decimal SquareFootage { get; set; }
    public int YearBuilt { get; set; }
    public string MLSNumber { get; set; }
    public Dictionary<string, Dictionary<string, List<string>>> Features { get; set; }
    public DateTime DateCreated { get; set; }
}
