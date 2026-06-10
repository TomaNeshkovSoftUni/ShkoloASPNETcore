using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShkoloASPNETcore.Infrastructure.Data.Models
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public string ApplicationUserId { get; set; } = null!;

        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser ApplicationUser { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Department { get; set; } = null!;

        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    }
}
