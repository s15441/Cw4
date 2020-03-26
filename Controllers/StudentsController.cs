using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cw3.DAL;
using Cw3.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cw3.Controllers
{
    [ApiController]
    [Route("api/students")] 
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStudent(string orderBy)
        {
             List<Student> _students = new List<Student>();
            using(var connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=S15441;Integrated Security=True"))
            using(var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT * FROM STUDENT";

                connection.Open();
                var dr = command.ExecuteReader();
                int idStudent = 1;
                while (dr.Read())
                {
                    var st = new Student();
                    st.IdStudent = idStudent++;
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    _students.Add(st);
                }
            }   
            return Ok(_students);
        }
        [HttpGet("{IndexNumber}")]
        public IActionResult GetStudents(string IndexNumber)
        {
            using (var client = new SqlConnection("Data Source=db-mssql;Initial Catalog=s15441;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = client;
                com.CommandText = $"select Student.IndexNumber,FirstName, LastName, Enrollment.IdEnrollment, " +
                    $"Semester, IdStudy  from Enrollment INNER JOIN Student ON Student.IdEnrollment = Enrollment.IdEnrollment WHERE Student.IndexNumber =  @IndexNumber ";
                com.Parameters.AddWithValue("IndexNumber", IndexNumber);
                client.Open();
                var dr = com.ExecuteReader();
                string toReturn = "";
    
                while (dr.Read())
                {
                    toReturn += dr["IndexNumber"].ToString() + " " + dr["FirstName"].ToString() + " " + dr["LastName"].ToString() + " " + dr["IdEnrollment"].ToString() +
                        "semester " + dr["Semester"].ToString() + " " + dr["IdStudy"];
                }

                return Ok(toReturn);
            }
        }
        private readonly IDbsService _dbService;

        public StudentsController(IDbsService dbService)
        {
            _dbService = dbService;
        }

        [HttpPut("{id}")]
        public IActionResult PutStudent(int id, Student student)
        {

            return Ok("Aktualizacja dokonczona");
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id, Student student)
        {
            return Ok("Usuwanie ukonczone");
        }
    }

}