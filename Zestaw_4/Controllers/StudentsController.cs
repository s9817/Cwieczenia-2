using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zestaw_4.Models;
using Zestaw_4.Services;

namespace Zestaw_4.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private const string ConString = "Data Source=db-mssql;Initial Catalog=s9817;Integrated Security=True";

        // pierwszy sposób na wstrzyknięcie zależności
        private IStudentsDal _dbService;
        public StudentsController(IStudentsDal dbService)
        {
            _dbService = dbService;
        }

        //Zadanie 4.1 - pobieranie danych - dodawanie połaczenie
        //Zadanie 4.2 - pobieranie danych - dodanie obiektu Sql-Command
        // drugi sposób na wstrzyknięcie zależności bezpośrednio w parametrze metody GetStudents()
        [HttpGet]
        public IActionResult getStudents([FromServices] IStudentsDal dbService)
        {
            var list = new List<Student>();
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {

                com.Connection = con;
                com.CommandText = "select * from student2";

                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = dr["BirthDate"].ToString();
                    st.IdEnrollment = (int)dr["IdEnrollment"];
                    list.Add(st);
                }
            }
            return Ok(list);
        }


        //Zadanie 4.3 - pobieranie danych - koncówka zwracajace wpisy na semestr
        // dodatkowo zabezpieczenie przed atakiem typu SQL Incjection
        [HttpGet("{indexNumber}")]
        public IActionResult getStudent(string indexNumber)
        {
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select * from student2 WHERE IndexNumber=@index";

                com.Parameters.AddWithValue("index", indexNumber);

                con.Open();
                var dr = com.ExecuteReader();
                if (dr.Read())
                {
                    var st = new Student();

                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = dr["BirthDate"].ToString();
                    st.IdEnrollment = (int)dr["IdEnrollment"];
                    return Ok(st);
                }
            }
            return NotFound();
        }

        [HttpGet("{example2}")]
        public IActionResult GetStudents2()
        {
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "Proc_Wyk4";
                com.CommandType = System.Data.CommandType.StoredProcedure;

                com.Parameters.AddWithValue("LastName", "Lajnert");
                con.Open();

                var dr = com.ExecuteReader();
            }
            return NotFound();
        }

    }
 }