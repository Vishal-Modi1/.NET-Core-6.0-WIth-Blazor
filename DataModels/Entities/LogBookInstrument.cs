using System;

namespace DataModels.Entities
{
    public class LogBookInstrument
    {
        public long Id { get; set; }
        public long LogBookId { get; set; }
        public Int32 ActualInstrument { get; set; }
        public Int32 SimulatedInstrument { get; set; }
        public Int32 Holds { get; set; }
    }
}
