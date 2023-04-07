using Microsoft.AspNetCore.Components;
using Web.UI.Models.Document;

namespace Web.UI.Pages.Document.DocumentTag
{
    partial class TagsListFilter 
    {
        [Parameter] public LeftPanel LeftPanel { get; set; }
        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }

        async Task Filter()
        {
            LeftPanel.tagFilterParamteres.TagIds = string.Join(",", LeftPanel.documentTagsList.Where(p => p.IsSelected).Select(p => p.Id).ToList());
            await LeftPanel.CloseFilterDialog(true);
        }
    }
}
