using System;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            var query = @"select * from dbo.Employee";
            var dataTable = new DataTable();
            var sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            using (var sqlConnection = new SqlConnection(sqlDataSource))
            {
                sqlConnection.Open();
                using var sqlCommand = new SqlCommand(query, sqlConnection);
                var sqlDataReader = sqlCommand.ExecuteReader();
                dataTable.Load(sqlDataReader);
                sqlDataReader.Close();
                sqlConnection.Close();
            }

            return new JsonResult(dataTable);
        }

        [HttpPost]
        public JsonResult Post(Employee employee)
        {

            var query = @"insert into  dbo.Employee values('" + employee.EmployeeName + @"',
                                '" + employee.Department + @"','" + employee.DateOfJoining + @"','" +
                        employee.PhotoFileName + @"')";
            var sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            using (var sqlConnection = new SqlConnection(sqlDataSource))
            {
                sqlConnection.Open();
                using var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.ExecuteReader();
                sqlConnection.Close();
            }

            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(Employee employee)
        {

            var query = @"update  dbo.Employee set 
                            EmployeeName ='" + employee.EmployeeName + @"' 
                            ,Department ='" + employee.Department + @"' 
                            ,DateOfJoining ='" + employee.DateOfJoining + @"' 
                            where EmployeeId ='" + employee.EmployeeId + @"'";
            var sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            using (var sqlConnection = new SqlConnection(sqlDataSource))
            {
                sqlConnection.Open();
                using var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.ExecuteReader();
                sqlConnection.Close();
            }

            return new JsonResult("Update Successfully");
        }

        [HttpDelete]
        public JsonResult Delete(int id)
        {

            var query = @"delete from dbo.Employee where EmployeeId ='" + id + @"'";
            var sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            using (var sqlConnection = new SqlConnection(sqlDataSource))
            {
                sqlConnection.Open();
                using var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.ExecuteReader();
                sqlConnection.Close();
            }

            return new JsonResult("Delete Successfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                var filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }

        }


        [Route("GetAllDepartmentNames")]
        [HttpGet]
        public JsonResult GetAllDepartmentNames()
        {
            var query = @"select DepartmentName from dbo.Department";
            var dataTable = new DataTable();
            var sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            using (var sqlConnection = new SqlConnection(sqlDataSource))
            {
                sqlConnection.Open();
                using var sqlCommand = new SqlCommand(query, sqlConnection);
                var sqlDataReader = sqlCommand.ExecuteReader();
                dataTable.Load(sqlDataReader);
                sqlDataReader.Close();
                sqlConnection.Close();
            }

            return new JsonResult(dataTable);

        }
    }
}
