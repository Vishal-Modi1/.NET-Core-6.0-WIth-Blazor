using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.VM.LogBook
{
    public class LogBookDataVM
    {
        public long Id { get; set; }

        public string TailNo { get; set; }

        public string CompanyName { get; set; }

        public string Airport { get; set; }

        public short DayTakeoffs { get; set; }

        public short NightTakeoffs { get; set; }

        public short DayLandingsFullStop { get; set; }

        public short NightLandingsFullStop { get; set; }

        public string UserName { get; set; }

        public double TotalTime{ get; set; }

        public DateTime Date { get; set; }

        public int TotalRecords { get; set; }

        [NotMapped]
        public bool IsLoadingEditButton { get; set; }
    }
}
