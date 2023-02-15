﻿using Telerik.Blazor.Components;
using DataModels.VM.LogBook;
using DataModels.VM.Common;
using Web.UI.Extensions;
using Web.UI.Utilities;
using Utilities;
using Microsoft.AspNetCore.Components;
using DataModels.Enums;

namespace Web.UI.Pages.LogBook
{
    partial class LogBookList
    {
        [CascadingParameter] public TelerikGrid<LogBookDataVM> grid { get; set; }
        LogBookDatatableParams datatableParams;
        LogBookFilterVM logBookFilterVM;
        IList<LogBookDataVM> data;

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
            logBookFilterVM = new LogBookFilterVM();

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            //logBookFilterVM = await LogBookService.GetFiltersAsync(dependecyParams);
        }

        async Task LoadData(GridReadEventArgs args)
        {
            isGridDataLoading = true;

            datatableParams = new DatatableParams().Create(args, "DisplayName").Cast<LogBookDatatableParams>();


            datatableParams.UserId = logBookFilterVM.UserId;

            datatableParams.UserRole = await _currentUserPermissionManager.GetRole(AuthStat);
            datatableParams.SearchText = searchText;
            pageSize = datatableParams.Length;

            data = await LogBookService.ListAsync(dependecyParams, datatableParams);
            args.Total = data.Count() > 0 ? data[0].TotalRecords : 0;
            args.Data = data;

            data.ToList().ForEach(p =>
            {
                p.Date = DateConverter.ToLocal(p.Date, globalMembers.Timezone);
            });

            isGridDataLoading = false;
        }

        void CreateLogBook(long id)
        {

        }

        void OpenDeleteDialog(LogBookDataVM logBookDataVM)
        {
            isDisplayPopup = true;

            operationType = OperationType.Delete;
            //_companyData = companyVM;
            popupTitle = "Delete LogBook";
        }
    }
}