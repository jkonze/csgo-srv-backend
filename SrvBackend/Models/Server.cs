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
        public static async Task<Server> CreateAsync(string ip, ushort port, string password, string displayName)
        {
            var server = new Server(ip, port, password, displayName);
            await server.InitializeAsync();
            return server;
        }

        private Server(String ip, ushort port, string password, string displayName)
        {
            Ip = IPAddress.Parse(ip);
            Port = port;
            Password = password;
            DisplayName = displayName;
        }

        public string DisplayName { get; set; }

        private RCON _connection => new RCON(Ip, Port, Password);

        private Status _status;

        private string Password { get; }

        private SourceQueryInfo _info;
        private ServerQueryPlayer[] _players;

        public ServerQueryPlayer[] Players => _players;
        

        private async Task InitializeAsync()
        {
            _status = await GetStatusAsync();
            _info = await GetInfoAsync();
            _players = await GetPlayersAsync();
        }

        private async Task<Status> GetStatusAsync()
            => await _connection.SendCommandAsync<Status>("status");

        private async Task<SourceQueryInfo> GetInfoAsync()
            => await ServerQuery.Info(Ip, Port, ServerQuery.ServerType.Source) as SourceQueryInfo;

        private async Task<ServerQueryPlayer[]> GetPlayersAsync()
            => await ServerQuery.Players(Ip, Port);

        public string Name => _info.Name;

        public bool IsRunning => _info != null ? true : false;

        public string Map => _info.Map;

        public byte ActivePlayers => _info.Players;

        public byte MaxPlayers => _info.MaxPlayers;

        public string Thumbnail
        {
            get
            {
                var rnd = new Random();
                return $"https://picsum.photos/300/200?image={rnd.Next(0, 100).ToString()}";
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

        public bool IsVacSecured => _info.VAC == ServerVAC.Secured ? true : false;

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