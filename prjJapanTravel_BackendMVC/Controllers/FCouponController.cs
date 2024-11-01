using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using prjJapanTravel_BackendMVC.Models;
using prjJapanTravel_BackendMVC.Service.JWTservice;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace prjJapanTravel_BackendMVC.Controllers
{
    [EnableCors("All")]
    [Route("api/[controller]")]
    [ApiController]
    public class FCouponController : ControllerBase
    {
        private readonly JapanTravelContext _context;
        public FCouponController(JapanTravelContext context) 
        {
            _context = context;
        }
       
    }
}
