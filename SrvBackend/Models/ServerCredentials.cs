using System.ComponentModel.DataAnnotations;

namespace SrvBackend.Models {
    public class ServerCredentials{

        public string ServerCredentialsId {get; set;}

        public string DisplayName {get; set;}

        public string IpAddress { get; set;}

        public ushort Port {get; set;}

        public string Password {get; set;}

        public string AdminId {get; set;}

        public string ServerName {get; set;}
    }
}