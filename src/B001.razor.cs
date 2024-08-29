using MetaFrm.Extensions;
using MetaFrm.Razor.Essentials;
using MetaFrm.Service;
using MetaFrm.Web.Bootstrap;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using MetaFrm.Management.Razor.ViewModels;
using MetaFrm.Management.Razor.Models;
using MetaFrm.Database;

namespace MetaFrm.Management.Razor
{
    /// <summary>
    /// B001
    /// </summary>
    public partial class B001
    {
        #region Variable
        internal B001ViewModel A001ViewModel { get; set; } = Factory.CreateViewModel<B001ViewModel>();

        internal DataGridControl<MenuModel>? DataGridControl;

        internal IEnumerable<Data.DataRow>? ParentMenuItems;
        internal IEnumerable<Data.DataRow>? AssemblyItems;

        internal MenuModel SelectItem = new();
        #endregion


        #region Init
        /// <summary>
        /// OnAfterRenderAsync
        /// </summary>
        /// <param name="firstRender"></param>
        /// <returns></returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (!this.AuthState.IsLogin())
                    this.Navigation?.NavigateTo("/", true);

                this.A001ViewModel = await this.GetSession<B001ViewModel>(nameof(this.A001ViewModel));

                this.Search();

                this.StateHasChanged();
            }
        }
        #endregion


        #region Dictionary
        private void OnResultEventParentMenuItems(IEnumerable<Data.DataRow> dataRows) => this.ParentMenuItems = dataRows;
        private void OnResultEventAssemblyItems(IEnumerable<Data.DataRow> dataRows) => this.AssemblyItems = dataRows;
        #endregion


        #region IO
        private void New()
        {
            this.SelectItem = new();
        }

        private void OnSearch()
        {
            if (this.DataGridControl != null)
                this.DataGridControl.CurrentPageNumber = 1;

            this.Search();
        }
        private void Search()
        {
            Response response;

            try
            {
                if (this.A001ViewModel.IsBusy) return;

                this.A001ViewModel.IsBusy = true;

                ServiceData serviceData = new()
                {
                    Token = this.AuthState.Token()
                };
                serviceData["1"].CommandText = this.GetAttribute("Search");
                serviceData["1"].AddParameter(nameof(this.A001ViewModel.SearchModel.SEARCH_TEXT), DbType.NVarChar, 4000, this.A001ViewModel.SearchModel.SEARCH_TEXT);
                serviceData["1"].AddParameter("USER_ID", DbType.Int, 3, this.AuthState.UserID());
                serviceData["1"].AddParameter("PAGE_NO", DbType.Int, 3, this.DataGridControl != null ? this.DataGridControl.CurrentPageNumber : 1);
                serviceData["1"].AddParameter("PAGE_SIZE", DbType.Int, 3, this.DataGridControl != null && this.DataGridControl.PagingEnabled ? this.DataGridControl.PagingSize : int.MaxValue);

                response = serviceData.ServiceRequest(serviceData);

                if (response.Status == Status.OK)
                {
                    this.A001ViewModel.SelectResultModel.Clear();

                    if (response.DataSet != null && response.DataSet.DataTables.Count > 0)
                    {
                        foreach (var datarow in response.DataSet.DataTables[0].DataRows)
                        {
                            this.A001ViewModel.SelectResultModel.Add(new MenuModel
                            {
                                MENU_ID = datarow.Int(nameof(MenuModel.MENU_ID)),
                                PARENT_MENU_ID = datarow.Int(nameof(MenuModel.PARENT_MENU_ID)),
                                PARENT_NAME = datarow.String(nameof(MenuModel.PARENT_NAME)),
                                NAME = datarow.String(nameof(MenuModel.NAME)),
                                DESCRIPTION = datarow.String(nameof(MenuModel.DESCRIPTION)),
                                ICON = datarow.String(nameof(MenuModel.ICON)),
                                IS_VISIBLE = datarow.String(nameof(MenuModel.IS_VISIBLE)) == "Y",
                                ASSEMBLY_ID = datarow.Int(nameof(MenuModel.ASSEMBLY_ID)),
                                NAMESPACE = datarow.String(nameof(MenuModel.NAMESPACE)),
                                SORT1 = datarow.Int(nameof(MenuModel.SORT1)),
                                DEPTH = datarow.Int(nameof(MenuModel.DEPTH)),
                                SORT = datarow.Int(nameof(MenuModel.SORT)),
                            });
                        }
                    }
                }
                else
                {
                    if (response.Message != null)
                    {
                        this.ModalShow("경고", response.Message, new() { { "확인", Btn.Warning } }, null);
                    }
                }
            }
            catch (Exception e)
            {
                this.ModalShow("예외가 발생했습니다.", e.Message, new() { { "확인", Btn.Danger } }, null);
            }
            finally
            {
                this.A001ViewModel.IsBusy = false;
                this.SetSession(nameof(A001ViewModel), this.A001ViewModel);
            }
        }

        private void Save()
        {
            Response? response;
            string? value;

            response = null;

            try
            {
                if (this.A001ViewModel.IsBusy)
                    return;

                if (this.SelectItem == null)
                    return;

                this.A001ViewModel.IsBusy = true;

                ServiceData serviceData = new()
                {
                    TransactionScope = true,
                    Token = this.AuthState.Token()
                };
                serviceData["1"].CommandText = this.GetAttribute("Save");
                serviceData["1"].AddParameter(nameof(this.SelectItem.MENU_ID), DbType.Int, 3, "2", nameof(this.SelectItem.MENU_ID), this.SelectItem.MENU_ID);
                serviceData["1"].AddParameter(nameof(this.SelectItem.PARENT_MENU_ID), DbType.Int, 3, this.SelectItem.PARENT_MENU_ID);
                serviceData["1"].AddParameter(nameof(this.SelectItem.NAME), DbType.NVarChar, 50, this.SelectItem.NAME);
                serviceData["1"].AddParameter(nameof(this.SelectItem.DESCRIPTION), DbType.NVarChar, 4000, this.SelectItem.DESCRIPTION);
                serviceData["1"].AddParameter(nameof(this.SelectItem.ICON), DbType.NVarChar, 250, this.SelectItem.ICON);
                serviceData["1"].AddParameter(nameof(this.SelectItem.IS_VISIBLE), DbType.NVarChar, 1, this.SelectItem.IS_VISIBLE ? "Y" : "N");
                serviceData["1"].AddParameter(nameof(this.SelectItem.ASSEMBLY_ID), DbType.Int, 3, this.SelectItem.ASSEMBLY_ID);
                serviceData["1"].AddParameter(nameof(this.SelectItem.SORT), DbType.Int, 3, this.SelectItem.SORT);
                serviceData["1"].AddParameter("USER_ID", DbType.Int, 3, this.AuthState.UserID());

                response = serviceData.ServiceRequest(serviceData);

                if (response.Status == Status.OK)
                {
                    if (response.DataSet != null && response.DataSet.DataTables.Count > 0 && response.DataSet.DataTables[0].DataRows.Count > 0 && this.SelectItem != null && this.SelectItem.ASSEMBLY_ID == null)
                    {
                        value = response.DataSet.DataTables[0].DataRows[0].String("Value");

                        if (value != null)
                            this.SelectItem.MENU_ID = value.ToInt();
                    }

                    this.ToastShow("완료", this.Localization["{0}이(가) 등록되었습니다.", this.GetAttribute("Title")], Alert.ToastDuration.Long);
                }
                else
                {
                    if (response.Message != null)
                    {
                        this.ModalShow("경고", response.Message, new() { { "확인", Btn.Warning } }, null);
                    }
                }
            }
            catch (Exception e)
            {
                this.ModalShow("예외가 발생했습니다.", e.Message, new() { { "확인", Btn.Danger } }, null);
            }
            finally
            {
                this.A001ViewModel.IsBusy = false;

                if (response != null && response.Status == Status.OK)
                {
                    this.Search();
                    this.StateHasChanged();
                }
            }
        }

        private void Delete()
        {
            this.ModalShow("삭제", "삭제하시겠습니까?", new() { { "삭제", Btn.Danger }, { "취소", Btn.Primary } }, EventCallback.Factory.Create<string>(this, DeleteAction));
        }
        private void DeleteAction(string action)
        {
            Response? response;

            response = null;

            try
            {
                if (action != "삭제") return;

                if (this.A001ViewModel.IsBusy) return;

                if (this.SelectItem == null) return;

                this.A001ViewModel.IsBusy = true;

                if (this.SelectItem.MENU_ID == null || this.SelectItem.MENU_ID <= 0)
                {
                    this.ToastShow("경고", this.Localization["삭제할 {0}을(를) 선택하세요.", this.GetAttribute("Title")], Alert.ToastDuration.Long);
                    return;
                }

                ServiceData serviceData = new()
                {
                    TransactionScope = true,
                    Token = this.AuthState.Token()
                };
                serviceData["1"].CommandText = this.GetAttribute("Delete");
                serviceData["1"].AddParameter(nameof(this.SelectItem.MENU_ID), DbType.Int, 3, this.SelectItem.MENU_ID);
                serviceData["1"].AddParameter("USER_ID", DbType.Int, 3, this.AuthState.UserID());

                response = serviceData.ServiceRequest(serviceData);

                if (response.Status == Status.OK)
                {
                    this.New();
                    this.ToastShow("완료", this.Localization["{0}이(가) 삭제되었습니다.", this.GetAttribute("Title")], Alert.ToastDuration.Long);
                }
                else
                {
                    if (response.Message != null)
                    {
                        this.ModalShow("경고", response.Message, new() { { "확인", Btn.Warning } }, null);
                    }
                }
            }
            catch (Exception e)
            {
                this.ModalShow("예외가 발생했습니다.", e.Message, new() { { "확인", Btn.Danger } }, null);
            }
            finally
            {
                this.A001ViewModel.IsBusy = false;

                if (response != null && response.Status == Status.OK)
                {
                    this.Search();
                    this.StateHasChanged();
                }
            }
        }
        #endregion


        #region Event
        private void SearchKeydown(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                this.OnSearch();
            }
        }

        private void RowModify(MenuModel item)
        {
            this.SelectItem = new()
            {
                MENU_ID = item.MENU_ID,
                PARENT_MENU_ID = item.PARENT_MENU_ID,
                PARENT_NAME = item.PARENT_NAME,
                NAME = item.NAME,
                DESCRIPTION = item.DESCRIPTION,
                ICON = item.ICON,
                IS_VISIBLE = item.IS_VISIBLE,
                ASSEMBLY_ID = item.ASSEMBLY_ID,
                NAMESPACE = item.NAMESPACE,
                SORT1 = item.SORT1,
                DEPTH = item.DEPTH,
                SORT = item.SORT,
            };

            this.ParentMenuItems = null;
        }

        private void Copy()
        {
            if (this.SelectItem != null)
            {
                this.SelectItem.MENU_ID = null;
                this.SelectItem.ASSEMBLY_ID = null;
                this.SelectItem.NAMESPACE = null;
            }
        }
        #endregion
    }
}