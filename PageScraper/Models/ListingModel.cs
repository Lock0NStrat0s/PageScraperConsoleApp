using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageScraper.Models;

public class ListingModel
{
    public int ListingId { get; set; }
    public int PropertyId { get; set; }
    public int UserId { get; set; }
    public DateTime ListingDate { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
