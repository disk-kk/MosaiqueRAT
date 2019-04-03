using Client.Controllers.Tools;
using Client.Models;
using Client.Packets.ServerPackets;
using System.Diagnostics;
using System.IO;

namespace Client.Controllers
{
    static class ManageController
    {

        public static void setClientIdentifier(SetClientIdentifier packet, ClientMosaic client)
        {
            
        }

        public static void CloseClient()
        {
            Program.client.Exit();
        }
    }
}
