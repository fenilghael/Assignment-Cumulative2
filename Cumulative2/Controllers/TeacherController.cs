using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Web.Mvc;
using Cumulative2.Models;

namespace Cumulative2.Controllers
{
    /// <summary>
    /// Controller for managing Teacher-related actions, such as listing, creating, showing, and deleting teachers.
    /// </summary>
    public class TeacherController : Controller
    {
        /// <summary>
        /// Displays the index view of the Teacher controller.
        /// </summary>
        /// <returns>The view for the index page.</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Lists teachers based on an optional search keyword.
        /// </summary>
        /// <param name="SearchKeyword">An optional search string to filter teachers by name, ID, or other attributes.</param>
        /// <returns>A list of teachers that match the search keyword.</returns>
        public ActionResult List(string SearchKeyword = null)
        {
            TeacherDataController teacherController = new TeacherDataController();
            IEnumerable<Teacher> teacherList = teacherController.ListTeachers(SearchKeyword);
            return View(teacherList);
        }

        /// <summary>
        /// Displays the details of a specific teacher based on their ID.
        /// </summary>
        /// <param name="teacherId">The ID of the teacher whose details need to be displayed.</param>
        /// <returns>The teacher details view.</returns>
        public ActionResult Show(int teacherId)
        {
            Debug.WriteLine($"Show action called with ID: {teacherId}");
            TeacherDataController teacherController = new TeacherDataController();
            Teacher teacher = teacherController.FindTeacher(teacherId);

            if (teacher == null || teacher.TeacherId == 0)
            {
                Debug.WriteLine($"Teacher not found for ID: {teacherId}");
                TempData["ErrorMessage"] = "Teacher not found.";
                return RedirectToAction("List");
            }

            Debug.WriteLine($"Teacher found: ID = {teacher.TeacherId}, Name = {teacher.TeacherFname} {teacher.TeacherLname}");
            return View(teacher);
        }

        /// <summary>
        /// Displays the view to create a new teacher.
        /// </summary>
        /// <returns>The view for creating a new teacher.</returns>
        public ActionResult NewTeacher()
        {
            return View();
        }

        /// <summary>
        /// Displays the view to create a new teacher using AJAX.
        /// </summary>
        /// <returns>The AJAX view for creating a new teacher.</returns>
        public ActionResult Ajax_NewTeacher()
        {
            return View();
        }

        /// <summary>
        /// Creates a new teacher using the provided data and validates the input.
        /// </summary>
        /// <param name="firstName">The first name of the teacher.</param>
        /// <param name="lastName">The last name of the teacher.</param>
        /// <param name="employeeNum">The employee number of the teacher.</param>
        /// <param name="hireDate">The hire date of the teacher.</param>
        /// <param name="salary">The salary of the teacher.</param>
        /// <returns>A view with a message indicating success or failure of teacher creation.</returns>
        [HttpPost]
        public ActionResult Create(string firstName, string lastName, string employeeNum, DateTime hireDate, decimal? salary)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(employeeNum) || hireDate == null || hireDate > DateTime.Now || salary == null || salary < 0)
            {
                ViewBag.Message = "Missing or incorrect information when adding a teacher.";
                return View("NewTeacher");
            }

            Teacher newTeacher = new Teacher
            {
                TeacherFname = firstName,
                TeacherLname = lastName,
                EmployeeNumber = employeeNum,
                HireDate = hireDate,
                Salary = salary ?? 0
            };

            TeacherDataController teacherController = new TeacherDataController();
            teacherController.AddTeacher(newTeacher);

            return RedirectToAction("List");
        }

        /// <summary>
        /// Confirms the deletion of a teacher by displaying their details.
        /// </summary>
        /// <param name="teacherId">The ID of the teacher to be deleted.</param>
        /// <returns>A confirmation view for deleting the teacher.</returns>
        public ActionResult DeleteConfirmation(int teacherId)
        {
            TeacherDataController teacherController = new TeacherDataController();
            Teacher teacher = teacherController.FindTeacher(teacherId);
            return View(teacher);
        }

        /// <summary>
        /// Displays the confirmation view for deleting a teacher using AJAX.
        /// </summary>
        /// <param name="teacherId">The ID of the teacher to be deleted via AJAX.</param>
        /// <returns>A view to confirm the deletion of the teacher via AJAX.</returns>
        public ActionResult Ajax_DeleteConfirmation(int teacherId)
        {
            TeacherDataController teacherController = new TeacherDataController();
            Teacher teacher = teacherController.FindTeacher(teacherId);
            return View(teacher);
        }

        /// <summary>
        /// Deletes a teacher based on their ID.
        /// </summary>
        /// <param name="teacherId">The ID of the teacher to be deleted.</param>
        /// <returns>An HTTP status indicating success or failure of the deletion.</returns>
        [HttpPost]
        public ActionResult Delete(int teacherId)
        {
            TeacherDataController teacherController = new TeacherDataController();
            teacherController.DeleteTeacher(teacherId);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}

/// <summary>
/// Routes to a dynamically generated "Update Teacher" page. Gathers teacher information from the database.
/// </summary>
/// <param name="teacherId">The ID of the teacher to update.</param>
/// <returns>A view populated with the current teacher data.</returns>
/// <example>
/// GET /Teacher/Update/3
/// </example>
public ActionResult Update(int teacherId)
{
    TeacherDataController teacherController = new TeacherDataController();
    Teacher teacher = teacherController.FindTeacher(teacherId);

    if (teacher == null || teacher.TeacherId == 0)
    {
        TempData["ErrorMessage"] = "Teacher not found.";
        return RedirectToAction("List");
    }

    return View(teacher);
}

/// <summary>
/// Routes to a dynamically generated "Update Teacher" page using AJAX. Gathers teacher information from the database.
/// </summary>
/// <param name="teacherId">The ID of the teacher to update via AJAX.</param>
/// <returns>A view populated with the current teacher data.</returns>
/// <example>
/// GET /Teacher/Ajax_Update/3
/// </example>
public ActionResult Ajax_Update(int teacherId)
{
    TeacherDataController teacherController = new TeacherDataController();
    Teacher teacher = teacherController.FindTeacher(teacherId);

    if (teacher == null || teacher.TeacherId == 0)
    {
        TempData["ErrorMessage"] = "Teacher not found.";
        return RedirectToAction("List");
    }

    return View(teacher);
}

/// <summary>
/// Updates a teacher’s information based on user input.
/// </summary>
/// <param name="teacherId">The ID of the teacher being updated.</param>
/// <param name="firstName">The updated first name of the teacher.</param>
/// <param name="lastName">The updated last name of the teacher.</param>
/// <param name="employeeNum">The updated employee number of the teacher.</param>
/// <param name="hireDate">The updated hire date of the teacher.</param>
/// <param name="salary">The updated salary of the teacher.</param>
/// <returns>Redirects to the teacher's Show page if successful; returns to update view with error message if not.</returns>
/// <example>
/// POST /Teacher/Update/3
/// Form Data: { firstName: "Updated", lastName: "Name", employeeNum: "EMP123", hireDate: "2023-01-01", salary: 80000 }
/// </example>
[HttpPost]
public ActionResult Update(int teacherId, string firstName, string lastName, string employeeNum, DateTime hireDate, decimal? salary)
{
    TeacherDataController teacherController = new TeacherDataController();

    if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
        string.IsNullOrEmpty(employeeNum) || hireDate == null || hireDate > DateTime.Now || salary == null || salary < 0)
    {
        ViewBag.Message = "Missing or incorrect information when updating a teacher.";
        Teacher teacher = teacherController.FindTeacher(teacherId);
        return View("Update", teacher);
    }

    Teacher updatedTeacher = new Teacher
    {
        TeacherFname = firstName,
        TeacherLname = lastName,
        EmployeeNumber = employeeNum,
        HireDate = hireDate,
        Salary = salary ?? 0
    };

    teacherController.UpdateTeacher(teacherId, updatedTeacher);

    return RedirectToAction("Show", new { teacherId = teacherId });
}

