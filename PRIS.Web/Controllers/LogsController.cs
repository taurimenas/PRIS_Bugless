using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PRIS.Web.Models.Logs;
using PRIS.Web.Storage;

namespace PRIS.Web.Controllers
{
    public class LogsController : Controller
    {
        private readonly IRepository _repository;
        private readonly string _user;

        public LogsController(IRepository repository, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _user = httpContextAccessor.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;
        }
        public async Task<IActionResult> Index()
        {
            if (true)
            {
                List<Logs> logsModel = new List<Logs>();
                using (SqlConnection con = new SqlConnection("Server = (localdb)\\mssqllocaldb; Database = PrisDb; Trusted_Connection = True; MultipleActiveResultSets = true"))
                {
                    await con.OpenAsync();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Logs", con);
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        logsModel.Add(new Logs { Id = reader["Id"].ToString(), Level = reader["Level"].ToString(), Message = reader["Message"].ToString(), TimeStamp = reader["TimeStamp"].ToString(), Exception = reader["Exception"].ToString() });
                    }
                };
                return View(logsModel);
            }
            else
            {
                return View();
            }
            return View();
        }
    }
}
