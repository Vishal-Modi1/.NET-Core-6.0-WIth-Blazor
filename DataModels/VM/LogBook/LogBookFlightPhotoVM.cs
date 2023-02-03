using System;

namespace DataModels.VM.LogBook
{
    public class LogBookFlightPhotoVM
    {
        public long Id { get; set; }
        public long LogBookId { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }

        public string ImagePath { get; set; }
        public bool IsDeleted { get; set; }
        public long? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }

        public byte[] ImageData { get; set; }
    }
}
