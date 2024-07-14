using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageScraper.Models;

internal class ListingModel
{
    public int Id { get; set; }
    public int PropertyId { get; set; }
    public int UserId { get; set; }
    public DateTime ListingDate { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
