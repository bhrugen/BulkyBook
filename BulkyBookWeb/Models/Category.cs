using System.ComponentModel.DataAnnotations;

namespace BulkyBookWeb.Models
{
    public class Category
    {
        //[Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(0,100,ErrorMessage ="Range must be between 0 and 100!!")]
        public int? DisplayOrder { get; set; }
    }
}
