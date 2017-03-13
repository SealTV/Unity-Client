using Shared.DataPackages.Server;

namespace Client.Network.PackageHandlers
{
    public sealed class SetRoomPackageHandler : PackageHandler
    {
        public SetRoomPackageHandler(Game game, ServerPackage package) : base(game, package)
        {
        }

        public override void HandlePackage()
        {
            SetRoomPackage roomPackage = (SetRoomPackage) Package;
            Game.Init(roomPackage.Room.Width, roomPackage.Room.Height, roomPackage.Room.Units);
        }
    }
}