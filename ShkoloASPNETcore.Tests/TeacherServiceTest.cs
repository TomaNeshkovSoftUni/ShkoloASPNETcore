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
            var user = new ApplicationUser
            {
                Id = "учител-ид-1",
                UserName = "teacher1@shkolo.bg",
                Email = "teacher1@shkolo.bg",
                FirstName = "СистемноИме",
                LastName = "СистемнаФамилия"
            };
            Context.Users.Add(user);
            await Context.SaveChangesAsync();

            var teacher = new Teacher
            {
                FirstName = "Димитър",
                LastName = "Георгиев",
                Department = "Математика",
                ApplicationUserId = "учител-ид-1"
            };

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
            var user = new ApplicationUser
            {
                Id = "учител-ид-2",
                UserName = "teacher2@shkolo.bg",
                Email = "teacher2@shkolo.bg",
                FirstName = "СистемноИме",
                LastName = "СистемнаФамилия"
            };
            Context.Users.Add(user);

            var teacher = new Teacher
            {
                FirstName = "Мария",
                LastName = "Стоянова",
                Department = "История",
                ApplicationUserId = "учител-ид-2"
            };
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
            var teacher = new Teacher { FirstName = "Христо", LastName = "Ботев", Department = "Литература", ApplicationUserId = "u-xb" };
            Context.Teachers.Add(teacher);
            await Context.SaveChangesAsync();

            await _teacherService.DeleteTeacherAsync(teacher.Id);
            var result = await _teacherService.GetTeacherByIdAsync(teacher.Id);

            Assert.That(result, Is.Null);
        }
    }
}