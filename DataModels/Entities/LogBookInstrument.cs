using System;

namespace DataModels.Entities
{
    public class LogBookInstrument
    {
        public long Id { get; set; }
        public long LogBookId { get; set; }
        public short ActualInstrument { get; set; }
        public short SimulatedInstrument { get; set; }
        public short Holds { get; set; }
    }
}
