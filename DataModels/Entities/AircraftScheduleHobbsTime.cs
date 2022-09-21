using DataModels.CustomValidations;

namespace DataModels.Entities
{
    public class AircraftScheduleHobbsTime
    {
        public long Id { get; set; }

        public long AircraftScheduleId { get; set; }

        public long AircraftEquipmentTimeId { get; set; }

        public decimal OutTime { get; set; }

        [DateGreaterThan(nameof(OutTime), "Time in value must be greater than time out")]
        public decimal InTime { get; set; }

        public decimal TotalTime { get; set; }
    }
}
