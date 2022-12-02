using System;

namespace DataModels.VM.Common
{
    public class DropDownValues 
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class DropDownLargeValues
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }

    public class DropDownGuidValues
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }

    public class DropDownStringValues
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }

}
