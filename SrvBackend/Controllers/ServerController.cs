using CoreRCON;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SrvBackend.Models;

namespace SrvBackend.Controllers
{
    [Route("api/[controller]")]
    public class ServerController : Controller
    {
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult ServerInfo(string id){
            // Get User ID from HttpContext

            // Get server from DB => by id and userID

            // return server object

            return BadRequest();

        }
        [Authorize]
        [HttpPost]
        public IActionResult AddServer([FromBody] ServerCredentials srvCredentials) {
         // get User ID from Context
         
         // add user ID to credentials object

         // save credentials

         // connect to server and create server object

            return BadRequest();

        }

        [Authorize]
        [HttpPut]
        public IActionResult UpdateServer([FromBody] ServerCredentials srvCredentials){
            //get userId from Context
            
            // find server By ID && userId

            // udate credentials object

            // save credentails object

            // return Server Object
            
            return BadRequest();

        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteServer(string id){
            // get user from context

            // find servercredentials by userId && id

            // delete

            // return oK 
            
            return BadRequest();

        }

        [Authorize]
        [HttpGet("list")]
        public IActionResult ServerList(){
            // get userID from context

            // find all credentials by userId

            // return all server Ids as array
            
            return BadRequest();

        }

        [Authorize]
        [HttpPost("exec")]
        public IActionResult ExecuteCommand([FromBody] Command cmd){
            // find credentials by user id && server id

            // execute command

            // return result
            
            return BadRequest();

        }
    }
}