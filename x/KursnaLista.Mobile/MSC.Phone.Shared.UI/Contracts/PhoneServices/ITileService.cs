using System;

namespace MSC.Phone.Shared.Contracts.PhoneServices
{
    public class TileData
    {
        public string Title { get; set; }
        public Uri BackgroundImage { get; set; }
        public Uri SmallBackgroundImage { get; set; }
    }

    public interface ITileService
    {
        void CreateTile(string url, TileData tileData, bool supportsWideTile);
        bool TileExists(string url);
        void DeleteTile(string url);
    }
}
