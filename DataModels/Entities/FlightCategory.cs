using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.Entities
{
    public class FlightCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int? CompanyId { get; set; }
    }
}
