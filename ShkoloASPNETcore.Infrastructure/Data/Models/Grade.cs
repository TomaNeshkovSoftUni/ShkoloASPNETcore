using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ShkoloASPNETcore.Infrastructure.Models.ShkoloClone.Infrastructure.Data.Models;

namespace ShkoloASPNETcore.Infrastructure.Data.Models
{
    public class Grade
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(2.00, 6.00)]
        public decimal Value { get; set; }

        [Required]
        public int StudentId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; } = null!;

        [Required]
        public int SubjectId { get; set; }

        [ForeignKey(nameof(SubjectId))]
        public Subject Subject { get; set; } = null!;

        public DateTime DateIssued { get; set; }
    }
}
