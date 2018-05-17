using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CoreRCON;
using CoreRCON.PacketFormats;
using CoreRCON.Parsers.Standard;

namespace SrvBackend.Models
{
    public class Server
    {
        public Server(String ip, ushort port, string password, string displayName)
        {
            Ip = IPAddress.Parse(ip);
            Port = port;
            Password = password;
            DisplayName = displayName;
        }

        public string DisplayName { get; set; }

        private RCON _connection => new RCON(Ip, Port, Password);

        private Status _status
        {
            get
            {
                var task = _connection.SendCommandAsync<Status>("status");
                task.Wait();
                return task.Result;
            }
        }

        private string Password { get; }

        private SourceQueryInfo Info
        {
            get
            {
                Task<IQueryInfo> t = ServerQuery.Info(Ip, Port, ServerQuery.ServerType.Source);
                t.Wait();
                return (SourceQueryInfo) t.Result;
            }
        }

        public string Name => Info.Name;

        public bool IsRunning => Info != null ? true : false;

        public string Map => Info.Map;

        public byte ActivePlayers => Info.Players;

        public byte MaxPlayers => Info.MaxPlayers;

        public string Thumbnail
        {
            get
            {
                var rnd = new Random();
                return $"https://picsum.photos/300/200?image={rnd.Next(0, 100).ToString()}";
            }
        }

        public ServerQueryPlayer[] Players
        {
            get
            {
                if (ActivePlayers > 0)
                {
                    var task = ServerQuery.Players(Ip, Port);
                    task.Wait();
                    return task.Result;
                }

                return null;
            }
        }

        public string TopPlayer
        {
            get
            {
                if (ActivePlayers > 0)
                {
                    var topScore = Players.Max(x => x.Score);
                    return Players.FirstOrDefault(x => x.Score == topScore).Name;
                }

                return null;
            }
        }

        public bool IsVacSecured => Info.VAC == ServerVAC.Secured ? true : false;

        public string[] Tags => _status.Tags;

        public string Version => _status.Version;

        public string Status
        {
            get
            {
                if (_status.Hostname != null)
                {
                    return "Everything is ok.";
                }
                else
                {
                    return "Woah, something is wrong";
                }
            }
        }

        [Required] public IPAddress Ip { get; }

        [Required] public ushort Port { get; }
    }
}