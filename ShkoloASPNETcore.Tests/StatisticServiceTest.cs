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
            var studentUser = Seed.User("stat-s", "stat@s.bg");
            var student = Seed.Student("stat-s");
            Context.Users.Add(studentUser);
            Context.Students.Add(student);
            await Context.SaveChangesAsync();

            var count = await _statisticsService.GetTotalStudentsCountAsync();
            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetTotalTeachersCountAsync_ShouldReturnCorrectCount()
        {
            var teacherUser = Seed.User("stat-t", "stat@t.bg");
            var teacher = Seed.Teacher("stat-t");
            Context.Users.Add(teacherUser);
            Context.Teachers.Add(teacher);
            await Context.SaveChangesAsync();

            var count = await _statisticsService.GetTotalTeachersCountAsync();
            Assert.That(count, Is.EqualTo(1));
        }
    }
}