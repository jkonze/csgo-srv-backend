using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using CoreRCON;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using SrvBackend.Helpers;
using SrvBackend.Models;

namespace SrvBackend.Controllers
{
    [Route("api/[controller]")]
    public class ServerController : Controller
    {
        private readonly CSGOContext _ctx;

        public ServerController(CSGOContext ctx)
        {
            _ctx = ctx;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> ServerInfo(string id)
        {
            // Get User ID from HttpContext

            var _admin = new Admin(HttpContext.User.Claims);

            var serverCredentials = _ctx.Servers.Where(s => s.AdminId == _admin.AdminId).First(s => s.ServerCredentialsId == id);

            if (serverCredentials != null)
            {
                var server = await Server.CreateAsync(serverCredentials.IpAddress, serverCredentials.Port,
                    serverCredentials.Password, serverCredentials.DisplayName);
                return Ok(server);
            }

            return NotFound();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddServer([FromBody] ServerCredentials srvCredentials)
        {
            if (ModelState.IsValid)
            {
                var _admin = new Admin(HttpContext.User.Claims);
                // get User ID from Context

                // add user ID to credentials object
                srvCredentials.AdminId = _admin.AdminId;

                _ctx.Servers.Add(srvCredentials);
                // save credentials
                _ctx.SaveChanges();

                // connect to server and create server object
                var server = await Server.CreateAsync(srvCredentials.IpAddress, srvCredentials.Port, srvCredentials.Password,
                    srvCredentials.DisplayName);
                return Ok(server);
            }

            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateServer([FromBody] ServerCredentials srvCredentials)
        {
            //get userId from Context

            // find server By ID && userId
            if (_ctx.Servers.Any(x => x.ServerCredentialsId == srvCredentials.ServerCredentialsId))
            {
                _ctx.Servers.Update(srvCredentials);
                _ctx.SaveChanges();
            }

            // udate credentials object

            // save credentails object

            // return Server Object
            var server = Server.CreateAsync(srvCredentials.IpAddress, srvCredentials.Port, srvCredentials.Password,
                srvCredentials.DisplayName);
            return Ok(server);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServer(string id)
        {
            return await Task.Run(() =>
           {
               var _admin = new Admin(HttpContext.User.Claims);
               var server = _ctx.Servers.Where(x => x.AdminId == _admin.AdminId).First(x => x.ServerCredentialsId == id);
               _ctx.Servers.Remove(server);
               // get user from context

               // find servercredentials by userId && id

               // delete

               return Ok();
               // return oK  
           });
        }

        [Authorize]
        [HttpGet("list")]
        public IActionResult ServerList()
        {
            // get userID from context

            var _admin = new Admin(HttpContext.User.Claims);
            var relatedServers = _ctx.Servers.Where(x => x.AdminId == _admin.AdminId);

            List<string> serverIdList = new List<string>();
            foreach (var server in relatedServers)
            {
                serverIdList.Add(server.ServerCredentialsId);
            }
            // find all credentials by userId

            return Ok(relatedServers);

            // return all server Ids as array

            return BadRequest();
        }

        [Authorize]
        [HttpPost("exec")]
        public IActionResult ExecuteCommand([FromBody] Command cmd)
        {
            var _admin = new Admin(HttpContext.User.Claims);
            // find credentials by user id && server id
            var server = _ctx.Servers.Where(x => x.AdminId == _admin.AdminId)
                .First(x => x.ServerCredentialsId == cmd.ServerId);

            // execute command
            if (server != null)
            {
                // return result
                var rcon = new RCON(IPAddress.Parse(server.IpAddress), server.Port, server.Password);
                var res = rcon.SendCommandAsync(cmd.Cmd).Result;
                return Ok(res);
            }
            else
            {
                return NotFound();
            }
        }
    }
}