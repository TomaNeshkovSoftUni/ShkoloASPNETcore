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
        public async Task AddStudentAsync_ShouldSuccessfullySaveStudentToDatabase()
        {
            var student = new Student
            {
                FirstName = "Иван",
                LastName = "Иванов",
                EnrollmentNumber = "УЧ-12345"
            };
            var userId = "потребител-ид-1";

            await _studentService.AddStudentAsync(student, userId);
            var students = (await _studentService.GetAllStudentsAsync()).ToList();

            Assert.That(students.Count, Is.EqualTo(1));
            Assert.That(students[0].FirstName, Is.EqualTo("Иван"));
            Assert.That(students[0].LastName, Is.EqualTo("Иванов"));
            Assert.That(students[0].ApplicationUserId, Is.EqualTo("потребител-ид-1"));
        }

        [Test]
        public async Task GetAllStudentsAsync_ShouldReturnAllPersistedStudents()
        {
            var studentПърви = new Student
            {
                FirstName = "Георги",
                LastName = "Петров",
                EnrollmentNumber = "УЧ-00001",
                ApplicationUserId = "потребител-ид-2"
            };
            var studentВтори = new Student
            {
                FirstName = "Мария",
                LastName = "Димитрова",
                EnrollmentNumber = "УЧ-00002",
                ApplicationUserId = "потребител-ид-3"
            };

            Context.Students.Add(studentПърви);
            Context.Students.Add(studentВтори);
            await Context.SaveChangesAsync();

            var result = (await _studentService.GetAllStudentsAsync()).ToList();

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].FirstName, Is.EqualTo("Георги"));
            Assert.That(result[1].FirstName, Is.EqualTo("Мария"));
        }
    }
}