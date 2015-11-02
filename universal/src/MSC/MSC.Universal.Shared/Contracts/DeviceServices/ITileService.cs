using System;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.StartScreen;

namespace MSC.Universal.Shared.Contracts.DeviceServices
{
    public interface ITileService
    {
        void UpdateTile(XmlDocument tileXml, TimeSpan expiration);

        Task<bool> CreateTileAsync(SecondaryTile secondaryTile);
        bool TileExists(string secondaryTileId);
        Task<bool> DeleteTileAsync(string secondaryTileId);
    }
}
