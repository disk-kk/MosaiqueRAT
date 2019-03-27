using Serveur.Controllers.Server;

namespace Serveur.Models
{
    public class ClientRegistration
    {
        public string Ip { get; set; }

        public string Name { get; set; }

        public string AccType { get; set; }

        public string Country { get; set; }

        public string Os { get; set; }

        public string Status { get; set; }

        public ClientMosaic Client { get; set; }
    }
}
