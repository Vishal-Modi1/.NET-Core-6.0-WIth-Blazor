using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.VM
{
    public class AircraftScheduleDetailVM
    {
        public long Id { get; set; }

        public long AircraftScheduleId { get; set; }

        public string FlightStatus { get; set; }

        public DateTime CheckInTime { get; set; }

        public DateTime CheckOutTime { get; set; }

        public long? CheckInBy { get; set; }

        public long? CheckOutBy { get; set; }

        public bool IsCheckOut { get; set; }
    }
}
