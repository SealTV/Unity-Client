using Shared.DataPackages.Server;

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
