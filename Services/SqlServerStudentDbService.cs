using Cw4.DTOs.Requests;
using Cw4.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Cw4.Services
{
    public class SqlServerStudentDbService : IStudentsDbService
    {
        public Enrollment EnrollStudent(EnrollStudentRequest request)
        {
            Enrollment result = new Enrollment();
            using (var connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=S15441;Integrated Security=True"))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                connection.Open();
                var tran = connection.BeginTransaction();
                command.Transaction = tran;
                try
                {
                    //Studies check
                    command.CommandText = "SELECT IdStudy FROM STUDIES WHERE NAME=@NAME";
                    command.Parameters.AddWithValue("name", request.Studies);
                    command.Parameters.AddWithValue("stud", request.Studies);
                    command.Parameters.AddWithValue("Fname", request.FirstName);
                    command.Parameters.AddWithValue("Lname", request.LastName);
                    command.Parameters.AddWithValue("Bdate", request.BirthDate);
                    command.Parameters.AddWithValue("Index", request.IndexNumber);
                    var dr = command.ExecuteReader();

                    if (!dr.Read())
                    {
                        dr.Close();
                        tran.Rollback();
                        //return BadRequest("Studia nie istnieja");
                        return null;
                    }
                    int idStudies = (int)dr["IdStudy"];
                    dr.Close();

                    //checking enrollment
                    command.CommandText = "SELECT IdEnrollment FROM Enrollment inner join studies on Studies.IdStudy = Enrollment.IdStudy where Studies.Name = @stud and semester = 1 and startDate = (Select max(startdate) fRom enrollment)";

                    dr = command.ExecuteReader();
                    object id;
                    if (!dr.HasRows)
                    {
                        dr.Close();
                        command.CommandText = "INSERT INTO ENROLLMENT(IdEnrollment, semester, idstudy, startDate) values (SELECT(MAX(idEnrollment))+1,1,(select idstudy from studies where name=@stud),GETDATE())";
                        command.ExecuteNonQuery(); tran.Commit();
                        dr.Close();
                        command.CommandText = "SELECT(MAX(idEnrollment) From enrollment";
                        dr = command.ExecuteReader();
                        id = dr["IdEnrollment"];

                    }
                    else
                    {
                        dr.Read();
                        id = dr["IdEnrollment"];
                        dr.Close();
                    }
                    command.Parameters.AddWithValue("idEn", id);

                    command.CommandText = "SELECT indexNumber FROM student where IndexNumber = @index";
                    dr = command.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Close();
                        tran.Rollback();
                        //return BadRequest("indeks nie jest unikalny");
                        return null; 
                    }
                    dr.Close();
                    //adding a student
                    command.CommandText = "INSERT INTO STUDENT (IndexNumber, FirstName, LastName, BirthDate,idEnrollment) VALUES(@Index, @Fname, @Lname, @Bdate, @idEn)";

                    Console.Write(command.ExecuteNonQuery());
                    dr.Close();


                    command.CommandText = "select startDate from enrollment where idEnrollment = @iden";
                    dr = command.ExecuteReader();
                    dr.Read();
                    result.idEnrollment = (int)id;
                    result.Semester = 1;
                    result.IdStudy = idStudies;
                    result.StartDate = (DateTime)dr["startDate"];
                    dr.Close();
                    tran.Commit();
                }
                catch (SqlException exc)
                {
                    Console.WriteLine(exc.Message);
                    tran.Rollback();
                }
            }

            return result;

        }

        public Enrollment PromoteStudents(PromoteRequest request)
        {
            Enrollment result = new Enrollment();
            using (var connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=S15441;Integrated Security=True"))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                connection.Open();
                var tran = connection.BeginTransaction();
                command.Transaction = tran;
                try
                {
                    command.CommandText = "SELECT * FROM ENROLLMENT WHERE SEMESTER = @Sem AND IDSTUDY = (SELECT IdStudy from Studies where name = @Name)";
                    command.Parameters.AddWithValue("Sem", request.Semester);
                    command.Parameters.AddWithValue("Name", request.Studies);
                    var dr = command.ExecuteReader();
                    if (!dr.HasRows)
                    {
                        return null;
                    }


                    dr.Close();
                    command.CommandText = "Exec promote @name , @sem";
                    command.ExecuteNonQuery();

                }
                catch (SqlException exc)
                {
                    Console.WriteLine(exc);
                    tran.Rollback();
                }
                return result;
            }
        }
    }
}
