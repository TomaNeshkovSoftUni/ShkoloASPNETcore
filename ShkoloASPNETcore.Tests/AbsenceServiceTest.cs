using NUnit.Framework;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Infrastructure.Data.Enums;
using ShkoloASPNETcore.Services;
using ShkoloASPNETcore.Tests.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShkoloASPNETcore.Tests
{
    [TestFixture]
    public class AbsenceServiceTests : TestBase
    {
        private AbsenceService _absenceService;

        [SetUp]
        public void ServiceSetUp()
        {
            _absenceService = new AbsenceService(Context);
        }

        [Test]
        public async Task GetAllAbsencesAsync_ShouldReturnEmpty_WhenNoneExist()
        {
            var result = await _absenceService.GetAllAbsencesAsync();
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task AddAbsenceAsync_ShouldPersistAbsence()
        {
            var studentUser = new ApplicationUser { Id = "s-abs", UserName = "s@abs.bg", Email = "s@abs.bg", FirstName = "ПотрИме", LastName = "ПотрФамилия" };
            var teacherUser = new ApplicationUser { Id = "t-abs", UserName = "t@abs.bg", Email = "t@abs.bg", FirstName = "ПотрИме", LastName = "ПотрФамилия" };
            Context.Users.AddRange(studentUser, teacherUser);

            var student = new Student { FirstName = "Петър", LastName = "Георгиев", EnrollmentNumber = "УЧ-999", ApplicationUserId = "s-abs" };
            var teacher = new Teacher { FirstName = "Иван", LastName = "Петров", Department = "Математика", ApplicationUserId = "t-abs" };
            Context.Students.Add(student);
            Context.Teachers.Add(teacher);
            await Context.SaveChangesAsync();

            var subject = new Subject { Name = "Математика", TeacherId = teacher.Id };
            Context.Subjects.Add(subject);
            await Context.SaveChangesAsync();

            var absence = new Absence
            {
                Type = AbsenceType.Неизвинено,
                StudentId = student.Id,
                SubjectId = subject.Id,
                DateIssued = DateTime.Now
            };

            await _absenceService.AddAbsenceAsync(absence);
            var absences = (await _absenceService.GetAllAbsencesAsync()).ToList();

            Assert.That(absences.Count, Is.EqualTo(1));
            Assert.That(absences[0].Type, Is.EqualTo(AbsenceType.Неизвинено));
            Assert.That(absences[0].StudentId, Is.EqualTo(student.Id));
            Assert.That(absences[0].SubjectId, Is.EqualTo(subject.Id));
        }
    }
}