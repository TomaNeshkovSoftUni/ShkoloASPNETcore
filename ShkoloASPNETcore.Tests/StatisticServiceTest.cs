using NUnit.Framework;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services;
using ShkoloASPNETcore.Tests.Infrastructure;
using System.Threading.Tasks;

namespace ShkoloASPNETcore.Tests
{
    [TestFixture]
    public class StatisticsServiceTests : TestBase
    {
        private StatisticsService _statisticsService;

        [SetUp]
        public void ServiceSetUp()
        {
            _statisticsService = new StatisticsService(Context);
        }

        [Test]
        public async Task GetTotalStudentsCountAsync_ShouldReturnCorrectCount()
        {
            var studentUser = new ApplicationUser { Id = "stat-s", UserName = "stat@s.bg", Email = "stat@s.bg", FirstName = "У", LastName = "У" };
            var student = new Student { FirstName = "Тест", LastName = "Ученик", ApplicationUserId = "stat-s" };
            Context.Users.Add(studentUser);
            Context.Students.Add(student);
            await Context.SaveChangesAsync();

            var count = await _statisticsService.GetTotalStudentsCountAsync();
            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetTotalTeachersCountAsync_ShouldReturnCorrectCount()
        {
            var teacherUser = new ApplicationUser { Id = "stat-t", UserName = "stat@t.bg", Email = "stat@t.bg", FirstName = "У", LastName = "У" };
            var teacher = new Teacher { FirstName = "Тест", LastName = "Учител", Department = "Общ", ApplicationUserId = "stat-t" };
            Context.Users.Add(teacherUser);
            Context.Teachers.Add(teacher);
            await Context.SaveChangesAsync();

            var count = await _statisticsService.GetTotalTeachersCountAsync();
            Assert.That(count, Is.EqualTo(1));
        }
    }
}