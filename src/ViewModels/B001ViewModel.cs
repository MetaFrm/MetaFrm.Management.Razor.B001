using MetaFrm.Management.Razor.Models;
using MetaFrm.MVVM;

namespace MetaFrm.Management.Razor.ViewModels
{
    /// <summary>
    /// A001ViewModel
    /// </summary>
    public partial class B001ViewModel : BaseViewModel
    {
        /// <summary>
        /// SearchModel
        /// </summary>
        public SearchModel SearchModel { get; set; } = new();

        /// <summary>
        /// SelectResultModel
        /// </summary>
        public List<MenuModel> SelectResultModel { get; set; } = new List<MenuModel>();

        /// <summary>
        /// C001ViewModel
        /// </summary>
        public B001ViewModel()
        {

        }
    }
}