using System;

namespace DataModels.VM.LogBook
{
    public class LogBookTrainingDetailVM
    {
        public long Id { get; set; }
        public long LogBookId { get; set; }
        public short DualGiven { get; set; }
        public short DualReceived { get; set; }
        public short SimulatedFlight { get; set; }
        public short GroundTraining { get; set; }
        public bool IsFlightReview { get; set; }
        public bool IsIPC { get; set; }
        public bool IsCheckRide { get; set; }
        public bool IsFAA { get; set; }
        public bool IsNVGProficiency { get; set; }
    }
}
