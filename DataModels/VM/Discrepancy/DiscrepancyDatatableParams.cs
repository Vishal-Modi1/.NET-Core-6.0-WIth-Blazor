﻿using DataModels.VM.Common;

namespace DataModels.VM.Discrepancy
{
    public class DiscrepancyDatatableParams : DatatableParams
    {
        public long AircraftId { get; set; }

        public bool IsOpen { get; set; } = true;
    }
}
