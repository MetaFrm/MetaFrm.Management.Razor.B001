using System.ComponentModel.DataAnnotations;

namespace MetaFrm.Management.Razor.Models
{
    /// <summary>
    /// MenuModel
    /// </summary>
    public class MenuModel
    {
        /// <summary>
        /// MENU_ID
        /// </summary>
        public int? MENU_ID { get; set; }

        /// <summary>
        /// PARENT_MENU_ID
        /// </summary>
        [Required]
        [Display(Name = "부모")]
        public int? PARENT_MENU_ID { get; set; }

        /// <summary>
        /// PARENT_NAME
        /// </summary>
        [Display(Name = "부모")]
        public string? PARENT_NAME { get; set; }

        /// <summary>
        /// NAME
        /// </summary>
        [Required]
        [Display(Name = "이름")]
        public string? NAME { get; set; }

        /// <summary>
        /// DESCRIPTION
        /// </summary>
        public string? DESCRIPTION { get; set; }

        /// <summary>
        /// ICON
        /// </summary>
        public string? ICON { get; set; }

        /// <summary>
        /// IS_VISIBLE
        /// </summary>
        public bool IS_VISIBLE { get; set; } = true;

        /// <summary>
        /// ASSEMBLY_ID
        /// </summary>
        public int? ASSEMBLY_ID { get; set; }

        /// <summary>
        /// NAMESPACE
        /// </summary>
        public string? NAMESPACE { get; set; }

        /// <summary>
        /// SORT1
        /// </summary>
        public int? SORT1 { get; set; }

        /// <summary>
        /// DEPTH
        /// </summary>
        public int? DEPTH { get; set; }

        /// <summary>
        /// SORT
        /// </summary>
        [Required]
        [Range(0, int.MaxValue)]
        [Display(Name = "정렬")]
        public int? SORT { get; set; }
    }
}