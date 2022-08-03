using System;

namespace DataModels.Entities
{
    public class DocumentTag : CommonField
    {
        public int Id { get; set; }

        public string TagName { get; set; }
    }
}
