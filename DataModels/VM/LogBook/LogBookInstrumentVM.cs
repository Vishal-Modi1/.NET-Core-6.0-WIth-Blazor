using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataModels.VM.Common;

namespace DataModels.VM.LogBook
{
    public class LogBookInstrumentVM
    {
        public long Id { get; set; }
        public long LogBookId { get; set; }
        public double ActualInstrument { get; set; }
        public double SimulatedInstrument { get; set; }
        public double Holds { get; set; }

        public List<DropDownSmallValues> Approaches { get; set; } = new ();

        [ValidateComplexType]
        public List<LogBookInstrumentApproachVM> LogBookInstrumentApproachesList { get; set; } = new();
    }
}
