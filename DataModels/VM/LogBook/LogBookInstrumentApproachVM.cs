﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.VM.LogBook
{
    public class LogBookInstrumentApproachVM
    {
        public long Id { get; set; }
        public long LogBookInstrumentId { get; set; }
        public string Airport { get; set; }
        public short InstrumentApproachId { get; set; }
        public string Runway { get; set; }
        public bool IsCircleToLand { get; set; }
        public string Comments { get; set; }
    }
}