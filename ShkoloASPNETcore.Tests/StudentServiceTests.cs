using NUnit.Framework;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services;
using ShkoloASPNETcore.Tests.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace ShkoloASPNETcore.Tests
{
    [TestFixture]
    public class StudentServiceTests : TestBase
    {
        private StudentService _studentService;

        [SetUp]
        public void ServiceSetUp()
        {
            _studentService = new StudentService(Context);
        }

        [Test]
        public async Task GetAllStudentsAsync_ShouldReturnEmptyList_WhenNoStudentsExist()
        {
            var result = await _studentService.GetAllStudentsAsync();
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetAllStudentsAsync_ShouldReturnStudents()
        {
            var studentUser = new ApplicationUser { Id = "s3", UserName = "s3", Email = "s3", FirstName = "Тест", LastName = "Тестов" };
            Context.Users.Add(studentUser);

            var student = new Student { FirstName = "Тест", LastName = "Тестов", ApplicationUserId = "s3" };
            Context.Students.Add(student);
            await Context.SaveChangesAsync();

            var students = await _studentService.GetAllStudentsAsync();

            Assert.That(students.Count(), Is.EqualTo(1));
            Assert.That(students.First().FirstName, Is.EqualTo("Тест"));
        }
    }
}