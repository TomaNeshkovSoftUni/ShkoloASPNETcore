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
    public class RemarkServiceTests : TestBase
    {
        private RemarkService _remarkService;

        [SetUp]
        public void ServiceSetUp()
        {
            _remarkService = new RemarkService(Context);
        }

        [Test]
        public async Task GetAllRemarksAsync_ShouldReturnEmpty_WhenNoneExist()
        {
            var result = await _remarkService.GetAllRemarksAsync();
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task AddRemarkAsync_ShouldSaveRemarkSuccessfully()
        {
            var studentUser = Seed.User("s-rem", "s@rem.bg");
            var teacherUser = Seed.User("t-rem", "t@rem.bg");
            Context.Users.AddRange(studentUser, teacherUser);

            var student = Seed.Student("s-rem");
            var teacher = Seed.Teacher("t-rem");
            Context.Students.Add(student);
            Context.Teachers.Add(teacher);
            await Context.SaveChangesAsync();

            var subject = Seed.Subject(teacher.Id);
            Context.Subjects.Add(subject);
            await Context.SaveChangesAsync();

            var remark = Seed.Remark(student.Id, subject.Id);
            remark.Comment = "Стабилно поведение в час.";
            remark.Label = "Забележка";
            remark.Issuer = "Иван Петров";

            await _remarkService.AddRemarkAsync(remark);

            var remarks = (await _remarkService.GetAllRemarksAsync()).ToList();

            Assert.That(remarks.Count, Is.EqualTo(1));
            Assert.That(remarks[0].Comment, Is.EqualTo("Стабилно поведение в час."));
            Assert.That(remarks[0].StudentId, Is.EqualTo(student.Id));
            Assert.That(remarks[0].SubjectId, Is.EqualTo(subject.Id));
        }

        [Test]
        public async Task GetRemarksByStudentIdAsync_ShouldReturnOnlyRemarksForTargetStudent()
        {
            var teacherUser = Seed.User("t-rem-test", "t@test.bg");
            var student1User = Seed.User("u-st", "st");
            var student2User = Seed.User("u-gg", "gg");
            Context.Users.AddRange(teacherUser, student1User, student2User);

            var teacher = Seed.Teacher("t-rem-test");
            Context.Teachers.Add(teacher);
            await Context.SaveChangesAsync();

            var subject = Seed.Subject(teacher.Id);
            Context.Subjects.Add(subject);

            var student = Seed.Student("u-st");
            var otherStudent = Seed.Student("u-gg");
            Context.Students.AddRange(student, otherStudent);
            await Context.SaveChangesAsync();

            var remark1 = Seed.Remark(student.Id, subject.Id);
            remark1.Comment = "Отличен проект";
            remark1.Label = "Похвала";
            remark1.Issuer = "Иван... Петров";

            var remark2 = Seed.Remark(otherStudent.Id, subject.Id);
            remark2.Comment = "Няма домашна";
            remark2.Label = "Забележка";
            remark2.Issuer = "Иван Петров";

            Context.Remarks.AddRange(remark1, remark2);
            await Context.SaveChangesAsync();

            var result = (await _remarkService.GetRemarksByStudentIdAsync(student.Id)).ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Comment, Is.EqualTo("Отличен проект"));
            Assert.That(result[0].StudentId, Is.EqualTo(student.Id));
        }
    }
}