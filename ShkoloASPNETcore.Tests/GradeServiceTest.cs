using NUnit.Framework;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services;
using ShkoloASPNETcore.Tests.Infrastructure;
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
            var studentUser = Seed.User("s1", "s1");
            var teacherUser = Seed.User("t1", "t1");
            Context.Users.AddRange(studentUser, teacherUser);

            var student = Seed.Student("s1");
            var teacher = Seed.Teacher("t1");
            Context.Students.Add(student);
            Context.Teachers.Add(teacher);
            await Context.SaveChangesAsync();

            var subject = Seed.Subject(teacher.Id);
            Context.Subjects.Add(subject);
            await Context.SaveChangesAsync();

            var grade = Seed.Grade(student.Id, subject.Id);
            grade.Value = 6.00m;
            grade.Issuer = "Петър Петров";

            await _gradeService.AddGradeAsync(grade);

            var grades = (await _gradeService.GetAllGradesAsync()).ToList();

            Assert.That(grades.Count, Is.EqualTo(1));
            Assert.That(grades[0].Value, Is.EqualTo(6.00m));
            Assert.That(grades[0].Issuer, Is.EqualTo("Петър Петров"));
        }

        [Test]
        public async Task DeleteGradeAsync_ShouldRemoveGradeFromDatabase()
        {
            var studentUser = Seed.User("s2", "s2");
            var teacherUser = Seed.User("t2", "t2");
            Context.Users.AddRange(studentUser, teacherUser);

            var student = Seed.Student("s2");
            var teacher = Seed.Teacher("t2");
            Context.Students.Add(student);
            Context.Teachers.Add(teacher);
            await Context.SaveChangesAsync();

            var subject = Seed.Subject(teacher.Id);
            Context.Subjects.Add(subject);
            await Context.SaveChangesAsync();

            var grade = Seed.Grade(student.Id, subject.Id);
            Context.Grades.Add(grade);
            await Context.SaveChangesAsync();

            await _gradeService.DeleteGradeAsync(grade.Id);
            var grades = await _gradeService.GetAllGradesAsync();

            Assert.That(grades, Is.Empty);
        }

        [Test]
        public async Task GetGradesByStudentIdAsync_ShouldReturnOnlyGradesForThatSpecificStudent()
        {
            var teacherUser = Seed.User("t3", "t3");
            var student1User = Seed.User("u-m1", "m1");
            var student2User = Seed.User("u-t2", "t2");
            Context.Users.AddRange(teacherUser, student1User, student2User);

            var teacher = Seed.Teacher("t3");
            Context.Teachers.Add(teacher);
            await Context.SaveChangesAsync();

            var subject = Seed.Subject(teacher.Id);
            Context.Subjects.Add(subject);

            var student1 = Seed.Student("u-m1");
            var student2 = Seed.Student("u-t2");
            Context.Students.AddRange(student1, student2);
            await Context.SaveChangesAsync();

            var grade1 = Seed.Grade(student1.Id, subject.Id);
            grade1.Value = 6.00m;

            var grade2 = Seed.Grade(student2.Id, subject.Id);
            grade2.Value = 4.00m;

            Context.Grades.AddRange(grade1, grade2);
            await Context.SaveChangesAsync();

            var result = (await _gradeService.GetGradesByStudentIdAsync(student1.Id)).ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Value, Is.EqualTo(6.00m));
            Assert.That(result[0].StudentId, Is.EqualTo(student1.Id));
        }
    }
}