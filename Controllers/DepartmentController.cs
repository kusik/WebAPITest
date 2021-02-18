using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            var query = @"select * from dbo.department";
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
        public JsonResult Post(Department dep)
        {

            var query = @"insert into  dbo.department values('" + dep.DepartmentName + @"')";
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
        public JsonResult Put(Department dep)
        {

            var query = @"update  dbo.department set DepartmentName ='" + dep.DepartmentName + @"' where DepartmentId ='" + dep.DepartmentId + @"'";
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
        public JsonResult Delete(int depId)
        {

            var query = @"delete from dbo.department where DepartmentId ='" + depId + @"'";
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

    }
}
