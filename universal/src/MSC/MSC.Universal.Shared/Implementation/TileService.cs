using System;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using MSC.Universal.Shared.Contracts.DeviceServices;

namespace MSC.Universal.Shared.Implementation
{
    public class TileService : ITileService
    {
        public void UpdateTile(XmlDocument tileXml, TimeSpan expiration)
        {
            var tileNotification = new TileNotification(tileXml) { ExpirationTime = DateTimeOffset.UtcNow + expiration };
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }

        public async Task<bool> CreateTileAsync(SecondaryTile secondaryTile)
        {
            return await secondaryTile.RequestCreateAsync().AsTask().ConfigureAwait(false);
        }

        public bool TileExists(string secondaryTileId)
        {
            return SecondaryTile.Exists(secondaryTileId);
        }

        public async Task<bool> DeleteTileAsync(string secondaryTileId)
        {
            var secondaryTile = new SecondaryTile(secondaryTileId);
            return await secondaryTile.RequestDeleteAsync().AsTask().ConfigureAwait(false);
        }
    }
}
