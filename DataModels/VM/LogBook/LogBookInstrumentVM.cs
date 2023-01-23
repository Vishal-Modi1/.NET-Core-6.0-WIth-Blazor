using System;
using System.Collections.Generic;

namespace DataModels.VM.LogBook
{
    public class LogBookInstrumentVM
    {
        public long Id { get; set; }
        public long LogBookId { get; set; }
        public Int32 ActualInstrument { get; set; }
        public Int32 SimulatedInstrument { get; set; }
        public Int32 Holds { get; set; }

        public List<LogBookInstrumentApproachVM> LogBookInstrumentApproachesVM { get; set; }
    }
}
