using Cw3.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.DAL
{
    public class MockDbService : IDbsService
    {
        private static IEnumerable<Student> _students;
        
        static MockDbService()
        {
            _students = new List<Student>
            {
                new Student{IdStudent=1, FirstName="Jan", LastName="Kowalski" },
                new Student{IdStudent=2, FirstName="Anna", LastName="Malewski" },
                new Student{IdStudent=3, FirstName="Andrzej", LastName="Andrzejewicz" }
            };

        }

        public bool CheckIndex(string index)
        {

            using (var client = new SqlConnection("Data Source=db-mssql;Initial Catalog=s15441;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = client;
                com.CommandText = $"Select * from student where idstudent = @id";
                com.Parameters.AddWithValue("index", index);
                client.Open();
                var dr = com.ExecuteReader();

                return dr.HasRows;

            }
        }
        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }
    }
}
