using DataModels.VM.Common;
using Microsoft.AspNetCore.Components;

namespace Web.UI.Shared.Components.TelerikMultiSelect
{
    partial class MultiSelectWithAllCheckbox
    {
        [Parameter] public List<DropDownLargeValues> Data { get; set; }
        [Parameter] public List<long> SelectedData { get; set; }
        [Parameter] public EventCallback<List<long>> OnChangeEventCallback { get; set; }
        [Parameter] public EventCallback<List<long>> UpdateParentListCallback { get; set; }
        [Parameter] public string PlaceHolderText { get; set; }

        bool IsAllSelected()
        {
            return SelectedData.Count == Data.Count;
        }

        bool GetChecked(long id)
        {
            return SelectedData.Any(p => p == id);
        }

        void ToggleSelectAll(bool selectAll)
        {
            if (selectAll)
            {
                SelectedData = Data.Select(p => p.Id).ToList();
            }
            else
            {
                SelectedData = new List<long>();
            }

            UpdateParentListCallback.InvokeAsync(SelectedData);
        }

        public void OnChange()
        {
            OnChangeEventCallback.InvokeAsync(SelectedData);
        }
    }
}
