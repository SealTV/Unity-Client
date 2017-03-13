using Shared.DataPackages.Client;
using Shared.DataPackages.Server;
using UnityEngine;

namespace Client.Network.PackageHandlers
{
    public class PongPackageHandler : PackageHandler
    {
        public PongPackageHandler(Game game, ServerPackage package) : base(game, package)
        {
        }

        public override void HandlePackage()
        {
            PongPackage pong = (PongPackage) Package;
            Debug.LogFormat("Pong package value {0}", pong.Value );

            Game.Client.SendPackage(new LoginPackage());
        }
    }
}
