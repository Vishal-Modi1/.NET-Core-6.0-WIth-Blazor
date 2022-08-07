using Microsoft.AspNetCore.Mvc.Rendering;
using DataModels.VM.Common;

namespace Web.UI.Utilities
{
    public static class DropDownListGenerator
    {
        public static List<SelectListItem> Generate(List<DropDownValues> list, string optionalText, int selectedOptionId=0)
        {
            List<SelectListItem> dropdownList = new List<SelectListItem>();

            if (list == null)
            {
                return dropdownList;
            }

            dropdownList.Add(new SelectListItem { Text = optionalText, Value = "" });

            dropdownList.AddRange(list.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.Name.ToString(),
                    Value = a.Id.ToString(),
                    Selected = a.Id == selectedOptionId ? true : false
                };
            }));

            return dropdownList;
        }

        public static List<SelectListItem> Generate(List<DropDownValues> list, string[] optionalText, int selectedOptionId=0)
        {
            List<SelectListItem> dropdownList = new List<SelectListItem>();

            if(list == null)
            {
                return dropdownList;
            }

            for (int i = 0; i < optionalText.Length; i++)
            {
                dropdownList.Add(new SelectListItem { Text = optionalText[i], Value = "" });
            }

            dropdownList.AddRange(list.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.Name.ToString(),
                    Value = a.Id.ToString(),
                    Selected = a.Id == selectedOptionId ? true : false
                };
            }));

            return dropdownList;
        }
    }
}
