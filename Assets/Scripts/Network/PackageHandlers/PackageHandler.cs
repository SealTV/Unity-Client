using Shared.DataPackages.Server;
using UnityEngine;

namespace Client.Network.PackageHandlers
{
    public abstract class PackageHandler
    {
        protected readonly Game Game;
        protected readonly ServerPackage Package;

        protected PackageHandler(Game game, ServerPackage package)
        {
            Game = game;
            Package = package;
            Debug.LogFormat("Package received: {0}", Package.Type);
        }

        public abstract void HandlePackage();

        public static PackageHandler GetPackageHandler(Game game, ServerPackage package)
        {
            switch (package.Type)
            {
                case ServerPackageType.Pong:
                    return new PongPackageHandler(game, package);
                case ServerPackageType.AcceptLogin:
                    return new AcceptLoginPackageHandler(game, package);
                case ServerPackageType.SetRoom:
                    return new SetRoomPackageHandler(game, package);
                case ServerPackageType.UpdatePositions:
                    return new UpdatePositionsPackageHandler(game, package);
            }

            return null;
        }
    }
}
