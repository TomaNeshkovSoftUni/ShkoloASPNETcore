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
            var studentUser = Seed.User("s3", "s3");
            Context.Users.Add(studentUser);

            var student = Seed.Student("s3");
            Context.Students.Add(student);
            await Context.SaveChangesAsync();

            var students = await _studentService.GetAllStudentsAsync();

            Assert.That(students.Count(), Is.EqualTo(1));
            Assert.That(students.First().FirstName, Is.EqualTo("Тест"));
        }

        [Test]
        public async Task GetStudentByUserIdAsync_ShouldReturnCorrectStudent_WhenUserExists()
        {
            var userId = "user-stud-123";
            var student = Seed.Student(userId);
            student.FirstName = "Алекс";
            student.LastName = "Попов";

            Context.Students.Add(student);
            await Context.SaveChangesAsync();

            var result = await _studentService.GetStudentByUserIdAsync(userId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.FirstName, Is.EqualTo("Алекс"));
        }

        [Test]
        public async Task GetStudentByIdAsync_ShouldReturnCorrectStudent()
        {
            var student = Seed.Student("u-el");
            student.FirstName = "Елена";
            student.LastName = "Георгиева";

            Context.Students.Add(student);
            await Context.SaveChangesAsync();

            var result = await _studentService.GetStudentByIdAsync(student.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.LastName, Is.EqualTo("Георгиева"));
        }

        [Test]
        public async Task DeleteStudentAsync_ShouldRemoveStudentFromDb()
        {
            var student = Seed.Student("u-del");
            Context.Students.Add(student);
            await Context.SaveChangesAsync();

            await _studentService.DeleteStudentAsync(student.Id);
            var result = await _studentService.GetStudentByIdAsync(student.Id);

            Assert.That(result, Is.Null);
        }
    }
}