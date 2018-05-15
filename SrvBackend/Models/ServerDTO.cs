using System.ComponentModel.DataAnnotations;

namespace SrvBackend.Models
{
    public class ServerDTO
    {
        [Required] public string IpAddress { get; set; }

        [Required] public ushort Port { get; set; }
    }
}