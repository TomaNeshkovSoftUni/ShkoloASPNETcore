using NUnit.Framework;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Infrastructure.Data.Enums;
using ShkoloASPNETcore.Services;
using ShkoloASPNETcore.Tests.Infrastructure;
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
            var studentUser = Seed.User("s-abs", "s@abs.bg");
            var teacherUser = Seed.User("t-abs", "t@abs.bg");
            Context.Users.AddRange(studentUser, teacherUser);

            var student = Seed.Student("s-abs");
            var teacher = Seed.Teacher("t-abs");
            Context.Students.Add(student);
            Context.Teachers.Add(teacher);
            await Context.SaveChangesAsync();

            var subject = Seed.Subject(teacher.Id);
            Context.Subjects.Add(subject);
            await Context.SaveChangesAsync();

            var absence = Seed.Absence(student.Id, subject.Id);
            absence.Type = AbsenceType.Закъснение;

            await _absenceService.AddAbsenceAsync(absence);
            var absences = (await _absenceService.GetAllAbsencesAsync()).ToList();

            Assert.That(absences.Count, Is.EqualTo(1));
            Assert.That(absences[0].Type, Is.EqualTo(AbsenceType.Закъснение));
            Assert.That(absences[0].StudentId, Is.EqualTo(student.Id));
            Assert.That(absences[0].SubjectId, Is.EqualTo(subject.Id));
        }
    }
}