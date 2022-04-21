using System.Collections.Generic;

namespace DataModels.VM.Common
{
    public class Menus
    {
        List<MenuItem> MenuItems { get; set; }
    }

    public class MenuItem
    {
        public string Action { get; set; }
        public string DisplayName { get; set; }
        public string Controller { get; set; }
        public string FavIconStyle { get; set; }
        public bool IsAdministrationModule { get; set; }
        public List<MenuItem> MenuItems { get; set; }
        public bool Expanded { get; set; }
        public int Index { get; set; }

        private string _path { get; set; }
        public string Path 
        {
            get { return string.Concat("/" ,Controller, "/" ,Action ) ; }
        }
    }
}