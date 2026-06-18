using NUnit.Framework;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services;
using ShkoloASPNETcore.Tests.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace ShkoloASPNETcore.Tests
{
    [TestFixture]
    public class TeacherServiceTests : TestBase
    {
        private TeacherService _teacherService;

        [SetUp]
        public void ServiceSetUp()
        {
            _teacherService = new TeacherService(Context);
        }

        [Test]
        public async Task GetAllTeachersAsync_ShouldReturnEmptyList_WhenNoTeachersExist()
        {
            var result = await _teacherService.GetAllTeachersAsync();
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task AddTeacherAsync_ShouldSuccessfullySaveTeacherToDatabase()
        {
            var user = Seed.User("учител-ид-1", "teacher1@shkolo.bg");
            Context.Users.Add(user);
            await Context.SaveChangesAsync();

            var teacher = Seed.Teacher("учител-ид-1");
            teacher.FirstName = "Димитър";
            teacher.LastName = "Георгиев";
            teacher.Department = "Математика";

            await _teacherService.AddTeacherAsync(teacher, "учител-ид-1");
            var teachers = (await _teacherService.GetAllTeachersAsync()).ToList();

            Assert.That(teachers.Count, Is.EqualTo(1));
            Assert.That(teachers[0].FirstName, Is.EqualTo("Димитър"));
            Assert.That(teachers[0].LastName, Is.EqualTo("Георгиев"));
            Assert.That(teachers[0].Department, Is.EqualTo("Математика"));
        }

        [Test]
        public async Task GetTeacherByIdAsync_ShouldReturnCorrectTeacher()
        {
            var user = Seed.User("учител-ид-2", "teacher2@shkolo.bg");
            Context.Users.Add(user);

            var teacher = Seed.Teacher("учител-ид-2");
            teacher.FirstName = "Мария";
            teacher.LastName = "Стоянова";
            teacher.Department = "История";

            Context.Teachers.Add(teacher);
            await Context.SaveChangesAsync();

            var result = await _teacherService.GetTeacherByIdAsync(teacher.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.FirstName, Is.EqualTo("Мария"));
            Assert.That(result.LastName, Is.EqualTo("Стоянова"));
            Assert.That(result.Department, Is.EqualTo("История"));
        }

        [Test]
        public async Task DeleteTeacherAsync_ShouldSuccessfullyRemoveTeacher()
        {
            var teacher = Seed.Teacher("u-xb");
            Context.Teachers.Add(teacher);
            await Context.SaveChangesAsync();

            await _teacherService.DeleteTeacherAsync(teacher.Id);
            var result = await _teacherService.GetTeacherByIdAsync(teacher.Id);

            Assert.That(result, Is.Null);
        }
    }
}