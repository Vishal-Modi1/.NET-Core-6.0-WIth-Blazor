using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Entities
{
    public class LogBookInstrumentApproach
    {
        public long Id { get; set; }
        public long LogBookInstrumentId { get; set; }
        public string Airport { get; set; }
        public long InstrumentApproachId { get; set; }
        public string Runway { get; set; }
        public bool IsCircleToLand { get; set; }
        public string Comments { get; set; }
    }
}
