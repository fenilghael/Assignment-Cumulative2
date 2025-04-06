using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using Cumulative2.Models;
using System.Diagnostics;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Cumulative2.Controllers
{
    /// <summary>
    /// Controller for handling API requests related to teacher data (CRUD operations).
    /// </summary>
    public class TeacherDataController : ApiController
    {
        private SchoolDbContext schoolDb = new SchoolDbContext();

        /// <summary>
        /// Lists all teachers or filters the list based on an optional search keyword.
        /// </summary>
        /// <param name="SearchKeyword">An optional keyword to filter teachers by name, hire date, or salary.</param>
        /// <returns>A collection of teachers that match the search criteria.</returns>
        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKeyword?}")]
        public IEnumerable<Teacher> ListTeachers(string SearchKeyword = null)
        {
            MySqlConnection conn = schoolDb.AccessDatabase();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Teachers WHERE LOWER(teacherfname) LIKE LOWER(@Search) OR LOWER(teacherlname) LIKE LOWER(@Search) OR LOWER(CONCAT(teacherfname, ' ', teacherlname)) LIKE LOWER(@Search) OR hiredate LIKE @Search OR DATE_FORMAT(hiredate, '%d-%m-%Y') LIKE @Search OR salary LIKE @Search";
            cmd.Parameters.AddWithValue("@Search", "%" + SearchKeyword + "%");
            cmd.Prepare();
            MySqlDataReader reader = cmd.ExecuteReader();
            List<Teacher> teachers = new List<Teacher>();
            while (reader.Read())
            {
                teachers.Add(new Teacher
                {
                    TeacherId = Convert.ToInt32(reader["teacherId"]),
                    TeacherFname = reader["teacherFname"].ToString(),
                    TeacherLname = reader["teacherLname"].ToString(),
                    EmployeeNumber = reader["employeenumber"].ToString(),
                    HireDate = Convert.ToDateTime(reader["hiredate"]),
                    Salary = Convert.ToDecimal(reader["salary"])
                });
            }
            conn.Close();
            return teachers;
        }

        /// <summary>
        /// Finds a teacher by their unique ID.
        /// </summary>
        /// <param name="teacherId">The unique ID of the teacher to be retrieved.</param>
        /// <returns>The teacher object with the specified ID, or null if not found.</returns>
        [HttpGet]
        [Route("api/TeacherData/FindTeacher/{teacherId}")]
        public Teacher FindTeacher(int teacherId)
        {
            Teacher teacher = null;
            MySqlConnection conn = schoolDb.AccessDatabase();
            try
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Teachers WHERE teacherid = @teacherId";
                cmd.Parameters.AddWithValue("@teacherId", teacherId);
                cmd.Prepare();
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    teacher = new Teacher
                    {
                        TeacherId = Convert.ToInt32(reader["teacherId"]),
                        TeacherFname = reader["teacherFname"].ToString(),
                        TeacherLname = reader["teacherLname"].ToString(),
                        EmployeeNumber = reader["employeenumber"].ToString(),
                        HireDate = Convert.ToDateTime(reader["hiredate"]),
                        Salary = Convert.ToDecimal(reader["salary"])
                    };
                    Debug.WriteLine($"Teacher data: ID = {teacher.TeacherId}, Name = {teacher.TeacherFname} {teacher.TeacherLname}");
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in FindTeacher: " + ex.Message);
                return null;
            }
            finally
            {
                conn.Close();
            }
            return teacher;
        }

        /// <summary>
        /// Adds a new teacher to the database.
        /// </summary>
        /// <param name="teacher">The teacher object containing details of the teacher to be added.</param>
        /// <returns>An HTTP response indicating the result of the operation.</returns>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        [Route("api/TeacherData/AddTeacher")]
        public IHttpActionResult AddTeacher([FromBody] Teacher teacher)
        {
            if (string.IsNullOrEmpty(teacher.TeacherFname) || string.IsNullOrEmpty(teacher.TeacherLname) ||
                string.IsNullOrEmpty(teacher.EmployeeNumber) || teacher.HireDate == null || teacher.HireDate > DateTime.Now || teacher.Salary < 0)
            {
                return BadRequest("Invalid data provided for adding the teacher.");
            }

            MySqlConnection conn = schoolDb.AccessDatabase();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) VALUES (@TeacherFname, @TeacherLname, @EmployeeNumber, @HireDate, @Salary)";
            cmd.Parameters.AddWithValue("@TeacherFname", teacher.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", teacher.TeacherLname);
            cmd.Parameters.AddWithValue("@EmployeeNumber", teacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@HireDate", teacher.HireDate);
            cmd.Parameters.AddWithValue("@Salary", teacher.Salary);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
            conn.Close();

            return Ok("Teacher added successfully");
        }

        /// <summary>
        /// Deletes a teacher from the database using their unique ID.
        /// </summary>
        /// <param name="teacherId">The ID of the teacher to be deleted.</param>
        /// <returns>An HTTP response indicating the result of the deletion operation.</returns>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        [Route("api/TeacherData/DeleteTeacher/{teacherId}")]
        public IHttpActionResult DeleteTeacher(int teacherId)
        {
            MySqlConnection conn = schoolDb.AccessDatabase();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM teachers WHERE teacherid = @teacherId";
            cmd.Parameters.AddWithValue("@teacherId", teacherId);

            try
            {
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();

                if (rowsAffected > 0)
                {
                    return Ok("Teacher deleted successfully");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine("Error deleting teacher: " + ex.Message);
                return InternalServerError();
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
    }
}
