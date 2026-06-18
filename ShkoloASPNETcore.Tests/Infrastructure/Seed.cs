using System;
using ShkoloASPNETcore.Infrastructure.Data.Enums;
using ShkoloASPNETcore.Infrastructure.Data.Models;

namespace ShkoloASPNETcore.Tests.Infrastructure
{
    internal static class Seed
    {
        public static ApplicationUser User(string id = "default-user-id", string email = "test@shkolo.bg") =>
            new()
            {
                Id = id,
                UserName = email,
                Email = email,
                FirstName = "Системно",
                LastName = "Име"
            };

        public static Student Student(string applicationUserId) =>
            new()
            {
                FirstName = "Тест",
                LastName = "Ученик",
                ApplicationUserId = applicationUserId
            };

        public static Teacher Teacher(string applicationUserId) =>
            new()
            {
                FirstName = "Тест",
                LastName = "Учител",
                Department = "Математика",
                ApplicationUserId = applicationUserId
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
                Issuer = "Системен Учител",
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
                Type = RemarkType.Забележка,
                Label = "Лоша дисциплина",
                Comment = "Тестова забележка",
                Issuer = "Системен Учител",
                DateIssued = DateTime.Now
            };
    }
}