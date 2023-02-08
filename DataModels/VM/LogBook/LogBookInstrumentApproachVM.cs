using System;
using System.ComponentModel.DataAnnotations;

namespace DataModels.VM.LogBook
{
    public class LogBookInstrumentApproachVM
    {
        public long Id { get; set; }
        public long LogBookInstrumentId { get; set; }

        [Required(ErrorMessage = "Airport is required")]
        public string Airport { get; set; }

        [Range(1, short.MaxValue, ErrorMessage = "Approach is required")]
        public short InstrumentApproachId { get; set; }

        public string Runway { get; set; }
        public bool IsCircleToLand { get; set; }
        public string Comments { get; set; }
    }
}
