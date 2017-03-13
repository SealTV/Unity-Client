using Shared.DataPackages.Server;

namespace Client.Network.PackageHandlers
{
    public sealed class UpdatePositionsPackageHandler : PackageHandler
    {
        public UpdatePositionsPackageHandler(Game game, ServerPackage package) : base(game, package)
        {
        }

        public override void HandlePackage()
        {
            throw new System.NotImplementedException();
        }
    }
}