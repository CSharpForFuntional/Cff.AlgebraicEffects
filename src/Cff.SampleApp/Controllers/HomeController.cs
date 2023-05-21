using Cff.SampleApp.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Cff.SampleApp.Controllers;

[ApiController]
public class EchoController : ControllerBase
{
    [HttpPost("/echo1")]
    public IActionResult Index(EchoDto echoDto)
    {
        


        return Ok(echoDto);
    }
}
