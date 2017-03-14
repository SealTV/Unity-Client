﻿using Shared.DataPackages.Server;

namespace Client.Network.PackageHandlers
{
    public sealed class UpdatePositionsPackageHandler : PackageHandler
    {
        public UpdatePositionsPackageHandler(Game game, ServerPackage package) : base(game, package)
        {
        }

        public override void HandlePackage()
        {
            UpdatePositionsPackage positionsPackage = (UpdatePositionsPackage) Package;
            foreach (var unit in positionsPackage.Units)
            {
                var unitComponent = Game.Units.Find(u => u.Unit.Id == unit.Id);
                unitComponent.Unit = unit;
            }
        }
    }
}