using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;


namespace ShkoloASPNETcore.Infrastructure.Models
{
    namespace ShkoloClone.Infrastructure.Data.Models
    {
        public class Student
        {
            [Key]
            public int Id { get; set; }

            [Required]
            public string ApplicationUserId { get; set; } = null!;

            [ForeignKey(nameof(ApplicationUserId))]
            public ApplicationUser ApplicationUser { get; set; } = null!;

            [Required]
            [MaxLength(20)]
            public string EnrollmentNumber { get; set; } = null!;

            public ICollection<Grade> Grades { get; set; } = new List<Grade>();
        }
    }
}
