using System;

namespace DataModels.Entities
{
    public class LogBookTrainingDetailVM
    {
        public long Id { get; set; }
        public long LogBookId { get; set; }
        public Int16 DualGiven { get; set; }
        public Int16 DualReceived { get; set; }
        public Int16 SimulatedFlight { get; set; }
        public Int16 GroundTraining { get; set; }
        public bool IsFlightReview { get; set; }
        public bool IsIPC { get; set; }
        public bool IsCheckRide { get; set; }
        public bool IsFAA { get; set; }
        public bool IsNVGProficiency { get; set; }
    }
}
