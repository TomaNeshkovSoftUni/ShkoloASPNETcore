using System.ComponentModel.DataAnnotations;

namespace ShkoloASPNETcore.Web.Models
{
    public class SubjectFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Името на предмета е задължително.")]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Моля, изберете учител.")]
        [Display(Name = "Учител")]
        public int TeacherId { get; set; }
    }
}