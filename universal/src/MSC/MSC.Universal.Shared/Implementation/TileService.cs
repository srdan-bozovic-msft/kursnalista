using System;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using MSC.Universal.Shared.Contracts.PhoneServices;

namespace MSC.Universal.Shared.Implementation
{
    public class TileService : ITileService
    {
        public void UpdateTile(XmlDocument tileXml, TimeSpan expiration)
        {
            var tileNotification = new TileNotification(tileXml) {ExpirationTime = DateTimeOffset.UtcNow + expiration};
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }
    }
}
