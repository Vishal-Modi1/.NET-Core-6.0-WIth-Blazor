using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.Entities
{
    public class Timezone
    {
        public short Id { get; set; }

        [Column("Timezone")]
        public string TimezoneName { get; set; }

        public string Offset { get; set; }
    }
}
