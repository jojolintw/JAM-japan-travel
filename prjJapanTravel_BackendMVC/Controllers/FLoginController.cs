using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjJapanTravel_BackendMVC.DTOs.Login;
using prjJapanTravel_BackendMVC.Models;

namespace prjJapanTravel_BackendMVC.Controllers
{
    [EnableCors("All")]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class FLoginController : ControllerBase
    {
        private readonly JapanTravelContext _context;
        public FLoginController(JapanTravelContext context) 
        {
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Member>>> GetLoginData(Login l)
        {





            return null;
        }
    }
}
