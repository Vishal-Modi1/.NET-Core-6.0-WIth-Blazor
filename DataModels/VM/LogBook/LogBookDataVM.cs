using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.VM.LogBook
{
    public class LogBookDataVM
    {
        public long Id { get; set; }

        public string TailNo { get; set; }

        public DateTime Date { get; set; }

        public int TotalRecords { get; set; }

        [NotMapped]
        public bool IsLoadingEditButton { get; set; }
    }
}
