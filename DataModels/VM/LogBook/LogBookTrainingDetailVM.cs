using System;

namespace DataModels.VM.LogBook
{
    public class LogBookTrainingDetailVM
    {
        public long Id { get; set; }
        public long LogBookId { get; set; }
        public double DualGiven { get; set; }
        public double DualReceived { get; set; }
        public double SimulatedFlight { get; set; }
        public double GroundTraining { get; set; }
        public bool IsFlightReview { get; set; }
        public bool IsIPC { get; set; }
        public bool IsCheckRide { get; set; }
        public bool IsFAA { get; set; }
        public bool IsNVGProficiency { get; set; }
    }
}
