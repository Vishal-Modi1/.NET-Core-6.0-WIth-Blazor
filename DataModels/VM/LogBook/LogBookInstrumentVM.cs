using System.Collections.Generic;
using DataModels.VM.Common;

namespace DataModels.VM.LogBook
{
    public class LogBookInstrumentVM
    {
        public long Id { get; set; }
        public long LogBookId { get; set; }
        public short ActualInstrument { get; set; }
        public short SimulatedInstrument { get; set; }
        public short Holds { get; set; }

        public List<DropDownSmallValues> Approaches { get; set; } = new ();
        public List<LogBookInstrumentApproachVM> LogBookInstrumentApproachesList { get; set; } = new();
    }
}
