using CoreRCON;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SrvBackend.Models;

namespace SrvBackend.Controllers
{
    [Route("api/[controller]")]
    public class ServerController : Controller
    {
        [HttpPost("info")]
        public IActionResult ServerInfo([FromBody] ServerDTO serverDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var server = new Server(serverDto.IpAddress, serverDto.Port);
            return Ok(server);
        }

        [HttpPost("info/detail")]
        public IActionResult ServerDetailedInfo([FromBody] ServerDTO serverDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var server = new Server(serverDto.IpAddress, serverDto.Port);
            return Ok(server);

        }

        [Authorize]
        [HttpPost("sendcommand")]
        public IActionResult SendCommand([FromBody] Command cmd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var admin = new ServerAdmin(HttpContext.User.Identity);

            var server = new Server(admin.ServerIp.ToString(), admin.ServerPort);
            var rcon = new RCON(server.Ip, server.Port, admin.Password);
            var result = rcon.SendCommandAsync(cmd.Cmd);

            return Ok(result);
        }
    }
}