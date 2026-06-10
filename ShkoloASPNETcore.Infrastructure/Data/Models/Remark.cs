using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShkoloASPNETcore.Infrastructure.Data.Enums;

namespace ShkoloASPNETcore.Infrastructure.Data.Models
{
    public class Remark
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Text { get; set; } = null!;

        [Required]
        public RemarkType Type { get; set; }

        [Required]
        public DateTime DateIssued { get; set; } = DateTime.Now;

        [Required]
        public int StudentId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; } = null!;

        [Required]
        public int SubjectId { get; set; }

        [ForeignKey(nameof(SubjectId))]
        public Subject Subject { get; set; } = null!;
    }
}