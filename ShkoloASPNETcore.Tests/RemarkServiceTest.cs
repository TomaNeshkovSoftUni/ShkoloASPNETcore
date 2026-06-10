using NUnit.Framework;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Infrastructure.Data.Enums;
using ShkoloASPNETcore.Services;
using ShkoloASPNETcore.Tests.Infrastructure;
using System;
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
            var studentUser = new ApplicationUser { Id = "s-rem", UserName = "s@rem.bg", Email = "s@rem.bg", FirstName = "ПотрИме", LastName = "ПотрФамилия" };
            var teacherUser = new ApplicationUser { Id = "t-rem", UserName = "t@rem.bg", Email = "t@rem.bg", FirstName = "ПотрИме", LastName = "ПотрФамилия" };
            Context.Users.AddRange(studentUser, teacherUser);

            var student = new Student { FirstName = "Николай", LastName = "Колев", ApplicationUserId = "s-rem" };
            var teacher = new Teacher { FirstName = "Иван", LastName = "Петров", Department = "История", ApplicationUserId = "t-rem" };
            Context.Students.Add(student);
            Context.Teachers.Add(teacher);
            await Context.SaveChangesAsync();

            var subject = new Subject { Name = "История", TeacherId = teacher.Id };
            Context.Subjects.Add(subject);
            await Context.SaveChangesAsync();

            var remark = new Remark
            {
                Text = "Стабилно поведение в час.",
                Type = RemarkType.Забележка,
                StudentId = student.Id,
                SubjectId = subject.Id,
                DateIssued = DateTime.Now
            };

            await _remarkService.AddRemarkAsync(remark);

            var remarks = (await _remarkService.GetAllRemarksAsync()).ToList();

            Assert.That(remarks.Count, Is.EqualTo(1));
            Assert.That(remarks[0].Text, Is.EqualTo("Стабилно поведение в час."));
            Assert.That(remarks[0].StudentId, Is.EqualTo(student.Id));
            Assert.That(remarks[0].SubjectId, Is.EqualTo(subject.Id));
        }
    }
}