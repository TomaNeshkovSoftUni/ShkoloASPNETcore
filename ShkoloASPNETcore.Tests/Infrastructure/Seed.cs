using ShkoloASPNETcore.Infrastructure.Data.Enums;
using ShkoloASPNETcore.Infrastructure.Data.Models;

namespace ShkoloASPNETcore.Tests.Infrastructure;

internal static class Seed
{
    public static Student Student(string uid = "uid1", string enrollment = "001") =>
        new()
        {
            FirstName = "Тест",
            LastName = "Ученик",
            ApplicationUserId = uid
        };

    public static Teacher Teacher(string uid = "tuid1") =>
        new()
        {
            FirstName = "Тест",
            LastName = "Учител",
            Department = "Математика",
            ApplicationUserId = uid
        };

    public static Subject Subject(int teacherId) =>
        new()
        {
            Name = "Математика",
            TeacherId = teacherId
        };

    public static Grade Grade(int studentId, int subjectId, decimal value = 5.50m) =>
        new()
        {
            StudentId = studentId,
            SubjectId = subjectId,
            Value = value,
            DateIssued = DateTime.Now
        };

    public static Absence Absence(int studentId, int subjectId) =>
        new()
        {
            StudentId = studentId,
            SubjectId = subjectId,
            Type = AbsenceType.Отсъствие,
            DateIssued = DateTime.Now
        };

    public static Remark Remark(int studentId, int subjectId) =>
        new()
        {
            StudentId = studentId,
            SubjectId = subjectId,
            Comment = "Тестова забележка",
            Type = RemarkType.Забележка,
            DateIssued = DateTime.Now
        };
}