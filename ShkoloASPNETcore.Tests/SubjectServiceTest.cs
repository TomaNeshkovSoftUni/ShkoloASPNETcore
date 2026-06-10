using NUnit.Framework;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services;
using ShkoloASPNETcore.Tests.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace ShkoloASPNETcore.Tests
{
    [TestFixture]
    public class SubjectServiceTests : TestBase
    {
        private SubjectService _subjectService;

        [SetUp]
        public void ServiceSetUp()
        {
            _subjectService = new SubjectService(Context);
        }

        [Test]
        public async Task GetAllSubjectsAsync_ShouldReturnEmpty_WhenNoSubjectsExist()
        {
            var result = await _subjectService.GetAllSubjectsAsync();
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task AddSubjectAsync_ShouldSaveSubjectCorrectly()
        {
            var user = new ApplicationUser { Id = "t-sub", UserName = "t@sub.bg", Email = "t@sub.bg", FirstName = "У", LastName = "У" };
            var teacher = new Teacher { FirstName = "Иван", LastName = "Петров", Department = "ИТ", ApplicationUserId = "t-sub" };
            Context.Users.Add(user);
            Context.Teachers.Add(teacher);
            await Context.SaveChangesAsync();

            var subject = new Subject { Name = "Бази Данни", TeacherId = teacher.Id };
            await _subjectService.AddSubjectAsync(subject);

            var subjects = (await _subjectService.GetAllSubjectsAsync()).ToList();
            Assert.That(subjects.Count, Is.EqualTo(1));
            Assert.That(subjects[0].Name, Is.EqualTo("Бази Данни"));
        }
    }
}