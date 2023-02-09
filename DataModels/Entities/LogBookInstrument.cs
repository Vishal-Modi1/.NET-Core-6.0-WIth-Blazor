using System;

namespace DataModels.Entities
{
    public class LogBookInstrument
    {
        public long Id { get; set; }
        public long LogBookId { get; set; }
        public double ActualInstrument { get; set; }
        public double SimulatedInstrument { get; set; }
        public double Holds { get; set; }
    }
}
