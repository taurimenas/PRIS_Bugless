using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PRIS.Web.Models.Logs;
using PRIS.Web.Storage;

namespace PRIS.Web.Controllers
{
    public class LogsController : Controller
    {
        private readonly string _user;
        private readonly IConfiguration _configuration;

        public LogsController(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _user = httpContextAccessor.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            List<Logs> logsModel = new List<Logs>();
            if (_user == "admin@akademija.it")
            {
                string defaultConnection = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection con = new SqlConnection(defaultConnection))
                {
                    await con.OpenAsync();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Logs", con);
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        logsModel.Add(new Logs { Id = reader["Id"].ToString(), Level = reader["Level"].ToString(), Message = reader["Message"].ToString(), TimeStamp = reader["TimeStamp"].ToString(), Exception = reader["Exception"].ToString() });
                    }
                    await con.CloseAsync();
                };
                return View(logsModel.OrderByDescending(x => x.TimeStamp));
            }
            else
            {
                return View(logsModel);
            }
        }
    }
}
