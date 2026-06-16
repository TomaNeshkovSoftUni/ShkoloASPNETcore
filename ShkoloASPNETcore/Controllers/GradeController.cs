using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services.Contracts;

namespace ShkoloASPNETcore.Controllers;

[Authorize]
public class GradeController : Controller
{
    private readonly IGradeService _gradeService;

    public GradeController(IGradeService gradeService)
    {
        _gradeService = gradeService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Add(int studentId)
    {
        var student = await _gradeService.GetStudentWithUserAsync(studentId);

        if (student == null)
        {
            return NotFound();
        }

        ViewBag.StudentName = $"{student.ApplicationUser.FirstName} {student.ApplicationUser.LastName}";
        ViewBag.StudentId = studentId;
        ViewBag.Subjects = await _gradeService.GetAllSubjectsAsync();

        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Add(Grade model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Subjects = await _gradeService.GetAllSubjectsAsync();
            return View(model);
        }

        await _gradeService.AddGradeAsync(model);

        return RedirectToAction("Details", "Student", new { id = model.StudentId });
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Delete(int id)
    {
        var studentId = await _gradeService.DeleteGradeAsync(id);

        if (studentId == null)
        {
            return NotFound();
        }

        return RedirectToAction("Details", "Student", new { id = studentId });
    }
}