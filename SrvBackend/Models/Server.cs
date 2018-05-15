using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using CoreRCON;
using CoreRCON.PacketFormats;

namespace SrvBackend.Models
{
    public class Server
    {
        public Server(String ip, ushort port)
        {
            Ip = IPAddress.Parse(ip);
            Port = port;
        }

        private SourceQueryInfo Info =>
            (SourceQueryInfo) ServerQuery.Info(Ip, Port, ServerQuery.ServerType.Source).Result;

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

        public ServerQueryPlayer[] Players => ServerQuery.Players(Ip, Port).Result;

        public string TopPlayer
        {
            get
            {
                var topScore = Players.Max(x => x.Score);
                return Players.FirstOrDefault(x => x.Score == topScore).Name;
            }
        }

        public bool IsVacSecured => Info.VAC == ServerVAC.Secured ? true : false;

        [Required] public IPAddress Ip { get; }

        [Required] public ushort Port { get; }
    }
}