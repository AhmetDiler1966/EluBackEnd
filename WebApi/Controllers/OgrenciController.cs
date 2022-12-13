using Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OgrenciController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public OgrenciController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        [HttpGet("getogrencibyNo")]
        public IActionResult GetOgrenci(string sOgrenciNo)
        {

            var result = _studentService.GetByOgrNo(sOgrenciNo);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getlistOgrenciDto")]
        public IActionResult GetListDto(string sOgrenciNo)
        {
            var result = _studentService.GetListDto(sOgrenciNo);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

    }
}
