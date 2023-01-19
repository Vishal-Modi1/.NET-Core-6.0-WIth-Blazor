using System;

namespace DataModels.Entities
{
    public class FlightTag : CommonField
    {
        public int Id { get; set; }

        public string TagName { get; set; }
    }
}
