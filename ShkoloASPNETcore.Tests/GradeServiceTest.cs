using NUnit.Framework;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services;
using ShkoloASPNETcore.Tests.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShkoloASPNETcore.Tests
{
    [TestFixture]
    public class GradeServiceTests : TestBase
    {
        private GradeService _gradeService;

        [SetUp]
        public void ServiceSetUp()
        {
            _gradeService = new GradeService(Context);
        }

        [Test]
        public async Task GetAllGradesAsync_ShouldReturnEmptyList_WhenNoGradesExist()
        {
            var result = await _gradeService.GetAllGradesAsync();
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task AddGradeAsync_ShouldSuccessfullySaveGrade()
        {
            var studentUser = new ApplicationUser { Id = "s1", UserName = "s1", Email = "s1", FirstName = "ПотрИме", LastName = "ПотрФамилия" };
            var teacherUser = new ApplicationUser { Id = "t1", UserName = "t1", Email = "t1", FirstName = "ПотрИме", LastName = "ПотрФамилия" };
            Context.Users.AddRange(studentUser, teacherUser);

            var student = new Student { FirstName = "Иван", LastName = "Иванов", ApplicationUserId = "s1" };
            var teacher = new Teacher { FirstName = "Петър", LastName = "Петров", Department = "БЕЛ", ApplicationUserId = "t1" };
            Context.Students.Add(student);
            Context.Teachers.Add(teacher);
            await Context.SaveChangesAsync();

            var subject = new Subject { Name = "Български език", TeacherId = teacher.Id };
            Context.Subjects.Add(subject);
            await Context.SaveChangesAsync();

            var grade = new Grade
            {
                Value = 6.00m,
                StudentId = student.Id,
                SubjectId = subject.Id,
                DateIssued = DateTime.Now
            };

            await _gradeService.AddGradeAsync(grade);

            var grades = (await _gradeService.GetAllGradesAsync()).ToList();

            Assert.That(grades.Count, Is.EqualTo(1));
            Assert.That(grades[0].Value, Is.EqualTo(6.00m));
        }

        [Test]
        public async Task DeleteGradeAsync_ShouldRemoveGradeFromDatabase()
        {
            var studentUser = new ApplicationUser { Id = "s2", UserName = "s2", Email = "s2", FirstName = "ПотрИме", LastName = "ПотрФамилия" };
            var teacherUser = new ApplicationUser { Id = "t2", UserName = "t2", Email = "t2", FirstName = "ПотрИме", LastName = "ПотрФамилия" };
            Context.Users.AddRange(studentUser, teacherUser);

            var student = new Student { FirstName = "Георги", LastName = "Георгиев", ApplicationUserId = "s2" };
            var teacher = new Teacher { FirstName = "Ангел", LastName = "Ангелов", Department = "ИТ", ApplicationUserId = "t2" };
            Context.Students.Add(student);
            Context.Teachers.Add(teacher);
            await Context.SaveChangesAsync();

            var subject = new Subject { Name = "Информационни технологии", TeacherId = teacher.Id };
            Context.Subjects.Add(subject);
            await Context.SaveChangesAsync();

            var grade = new Grade
            {
                Value = 5.50m,
                StudentId = student.Id,
                SubjectId = subject.Id,
                DateIssued = DateTime.Now
            };
            Context.Grades.Add(grade);
            await Context.SaveChangesAsync();

            await _gradeService.DeleteGradeAsync(grade.Id);
            var grades = await _gradeService.GetAllGradesAsync();

            Assert.That(grades, Is.Empty);
        }
    }
}