namespace DataModels.Entities
{
    public class ModuleDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string DisplayName { get; set; }
        public string Icon { get; set; }
        public int OrderNo { get; set; }
    }
}
