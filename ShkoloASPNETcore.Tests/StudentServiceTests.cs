using Microsoft.EntityFrameworkCore;
using ShkoloASPNETcore.Infrastructure.Data;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services;

namespace ShkoloASPNETcore.Tests
{
    // ─────────────────────────────────────────────────────────────────────────────
    //  Helper – creates a fresh, isolated in-memory DB for every test
    // ─────────────────────────────────────────────────────────────────────────────
    internal static class DbFactory
    {
        public static ShkoloDbContext Create(string dbName)
        {
            var options = new DbContextOptionsBuilder<ShkoloDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new ShkoloDbContext(options);
        }
    }

    // ─────────────────────────────────────────────────────────────────────────────
    //  StudentService tests
    // ─────────────────────────────────────────────────────────────────────────────
    [TestFixture]
    public class StudentServiceTests
    {
        // Each test uses a unique DB name so state never leaks between tests.
        private ShkoloDbContext _context = null!;
        private StudentService _service = null!;

        [SetUp]
        public void SetUp()
        {
            _context = DbFactory.Create($"StudentDb_{Guid.NewGuid()}");
            _service = new StudentService(_context);
        }

        [TearDown]
        public void TearDown() => _context.Dispose();

        // ── GetAllStudentsAsync ───────────────────────────────────────────────────

        [Test]
        public async Task GetAllStudentsAsync_EmptyDb_ReturnsEmptyList()
        {
            var result = await _service.GetAllStudentsAsync();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetAllStudentsAsync_WithSeededStudents_ReturnsAll()
        {
            _context.Students.AddRange(
                new Student { FirstName = "Иван", LastName = "Петров", EnrollmentNumber = "001", ApplicationUserId = "uid1" },
                new Student { FirstName = "Мария", LastName = "Иванова", EnrollmentNumber = "002", ApplicationUserId = "uid2" }
            );
            await _context.SaveChangesAsync();

            var result = await _service.GetAllStudentsAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
        }

        // ── AddStudentAsync ───────────────────────────────────────────────────────

        [Test]
        public async Task AddStudentAsync_ValidStudent_PersistsToDatabase()
        {
            var student = new Student
            {
                FirstName = "Георги",
                LastName = "Стоянов",
                EnrollmentNumber = "003"
            };

            await _service.AddStudentAsync(student, "user-abc");

            var saved = await _context.Students.FirstOrDefaultAsync();
            Assert.That(saved, Is.Not.Null);
            Assert.That(saved!.FirstName, Is.EqualTo("Георги"));
        }

        [Test]
        public async Task AddStudentAsync_SetsApplicationUserId_FromParameter()
        {
            var student = new Student
            {
                FirstName = "Петя",
                LastName = "Колева",
                EnrollmentNumber = "004"
            };

            await _service.AddStudentAsync(student, "expected-user-id");

            var saved = await _context.Students.FirstAsync();
            Assert.That(saved.ApplicationUserId, Is.EqualTo("expected-user-id"));
        }

        [Test]
        public async Task AddStudentAsync_IncreasesCount_ByOne()
        {
            int before = await _context.Students.CountAsync();

            await _service.AddStudentAsync(
                new Student { FirstName = "Тест", LastName = "Тестов", EnrollmentNumber = "005" },
                "uid3");

            int after = await _context.Students.CountAsync();
            Assert.That(after, Is.EqualTo(before + 1));
        }

        // ── GetStudentByIdAsync ───────────────────────────────────────────────────

        [Test]
        public async Task GetStudentByIdAsync_ExistingId_ReturnsCorrectStudent()
        {
            var student = new Student
            {
                FirstName = "Анна",
                LastName = "Димитрова",
                EnrollmentNumber = "006",
                ApplicationUserId = "uid4"
            };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            var result = await _service.GetStudentByIdAsync(student.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.LastName, Is.EqualTo("Димитрова"));
        }

        [Test]
        public async Task GetStudentByIdAsync_NonExistentId_ReturnsNull()
        {
            var result = await _service.GetStudentByIdAsync(9999);

            Assert.That(result, Is.Null);
        }

        // ── UpdateStudentAsync ────────────────────────────────────────────────────

        [Test]
        public async Task UpdateStudentAsync_ChangesFirstName_SavedCorrectly()
        {
            var student = new Student
            {
                FirstName = "Стар",
                LastName = "Вариант",
                EnrollmentNumber = "007",
                ApplicationUserId = "uid5"
            };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            student.FirstName = "Нов";
            await _service.UpdateStudentAsync(student);

            var updated = await _context.Students.FindAsync(student.Id);
            Assert.That(updated!.FirstName, Is.EqualTo("Нов"));
        }

        [Test]
        public async Task UpdateStudentAsync_ChangesEnrollmentNumber_SavedCorrectly()
        {
            var student = new Student
            {
                FirstName = "Тест",
                LastName = "Ъпдейт",
                EnrollmentNumber = "OLD-001",
                ApplicationUserId = "uid6"
            };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            student.EnrollmentNumber = "NEW-001";
            await _service.UpdateStudentAsync(student);

            var updated = await _context.Students.FindAsync(student.Id);
            Assert.That(updated!.EnrollmentNumber, Is.EqualTo("NEW-001"));
        }

        // ── DeleteStudentAsync ────────────────────────────────────────────────────

        [Test]
        public async Task DeleteStudentAsync_ExistingId_RemovesFromDatabase()
        {
            var student = new Student
            {
                FirstName = "За",
                LastName = "Изтриване",
                EnrollmentNumber = "008",
                ApplicationUserId = "uid7"
            };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            await _service.DeleteStudentAsync(student.Id);

            var result = await _context.Students.FindAsync(student.Id);
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task DeleteStudentAsync_NonExistentId_DoesNotThrow()
        {
            Assert.DoesNotThrowAsync(async () =>
                await _service.DeleteStudentAsync(9999));
        }

        [Test]
        public async Task DeleteStudentAsync_DecreasesCount_ByOne()
        {
            var student = new Student
            {
                FirstName = "Изтрий",
                LastName = "Мен",
                EnrollmentNumber = "009",
                ApplicationUserId = "uid8"
            };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            await _service.DeleteStudentAsync(student.Id);

            Assert.That(await _context.Students.CountAsync(), Is.EqualTo(0));
        }
    }

    // ─────────────────────────────────────────────────────────────────────────────
    //  StatisticsService tests
    // ─────────────────────────────────────────────────────────────────────────────
    [TestFixture]
    public class StatisticsServiceTests
    {
        private ShkoloDbContext _context = null!;
        private StatisticsService _service = null!;

        [SetUp]
        public void SetUp()
        {
            _context = DbFactory.Create($"StatsDb_{Guid.NewGuid()}");
            _service = new StatisticsService(_context);
        }

        [TearDown]
        public void TearDown() => _context.Dispose();

        // ── GetTotalStudentsCountAsync ────────────────────────────────────────────

        [Test]
        public async Task GetTotalStudentsCountAsync_EmptyDb_ReturnsZero()
        {
            var result = await _service.GetTotalStudentsCountAsync();

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public async Task GetTotalStudentsCountAsync_ThreeStudents_ReturnsThree()
        {
            _context.Students.AddRange(
                new Student { FirstName = "А", LastName = "А", EnrollmentNumber = "s1", ApplicationUserId = "u1" },
                new Student { FirstName = "Б", LastName = "Б", EnrollmentNumber = "s2", ApplicationUserId = "u2" },
                new Student { FirstName = "В", LastName = "В", EnrollmentNumber = "s3", ApplicationUserId = "u3" }
            );
            await _context.SaveChangesAsync();

            var result = await _service.GetTotalStudentsCountAsync();

            Assert.That(result, Is.EqualTo(3));
        }

        // ── GetTotalTeachersCountAsync ────────────────────────────────────────────

        [Test]
        public async Task GetTotalTeachersCountAsync_EmptyDb_ReturnsZero()
        {
            var result = await _service.GetTotalTeachersCountAsync();

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public async Task GetTotalTeachersCountAsync_TwoTeachers_ReturnsTwo()
        {
            _context.Teachers.AddRange(
                new Teacher { FirstName = "Проф", LastName = "Първи", ApplicationUserId = "t1" },
                new Teacher { FirstName = "Проф", LastName = "Втори", ApplicationUserId = "t2" }
            );
            await _context.SaveChangesAsync();

            var result = await _service.GetTotalTeachersCountAsync();

            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public async Task GetTotalTeachersCountAsync_AddingMoreTeachers_CountUpdates()
        {
            _context.Teachers.Add(
                new Teacher { FirstName = "Проф", LastName = "Три", ApplicationUserId = "t3" });
            await _context.SaveChangesAsync();

            _context.Teachers.Add(
                new Teacher { FirstName = "Проф", LastName = "Четири", ApplicationUserId = "t4" });
            await _context.SaveChangesAsync();

            var result = await _service.GetTotalTeachersCountAsync();

            Assert.That(result, Is.EqualTo(2));
        }

        // ── Both counts are independent ───────────────────────────────────────────

        [Test]
        public async Task BothCounts_AreIndependent_StudentsDoNotAffectTeachers()
        {
            _context.Students.AddRange(
                new Student { FirstName = "С1", LastName = "С1", EnrollmentNumber = "x1", ApplicationUserId = "u10" },
                new Student { FirstName = "С2", LastName = "С2", EnrollmentNumber = "x2", ApplicationUserId = "u11" }
            );
            _context.Teachers.Add(
                new Teacher { FirstName = "Т1", LastName = "Т1", ApplicationUserId = "t10" });
            await _context.SaveChangesAsync();

            var students = await _service.GetTotalStudentsCountAsync();
            var teachers = await _service.GetTotalTeachersCountAsync();

            Assert.That(students, Is.EqualTo(2));
            Assert.That(teachers, Is.EqualTo(1));
        }
    }
}