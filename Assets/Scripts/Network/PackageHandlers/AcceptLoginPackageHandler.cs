using Shared.DataPackages.Client;
using Shared.DataPackages.Server;
using UnityEngine;

namespace Client.Network.PackageHandlers
{
    public sealed class AcceptLoginPackageHandler : PackageHandler
    {
        public AcceptLoginPackageHandler(Game game, ServerPackage package) : base(game, package)
        {
        }

        public override void HandlePackage()
        {
            AcceptLoginPackage loginPackage = (AcceptLoginPackage) Package;
            Debug.LogFormat("Client id = {0}", loginPackage.ClientId);
            Game.Client.SendPackage(new GetRoomPackage());
        }
    }
}