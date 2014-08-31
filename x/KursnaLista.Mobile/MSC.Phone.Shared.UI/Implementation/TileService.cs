//using Microsoft.Phone.Shell;
//using MSC.Phone.Shared.Contracts.PhoneServices;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//
//namespace MSC.Phone.Shared
//{
//    public class TileService : ITileService
//    {
//        public void CreateTile(string url, ShellTileData tileData, bool supportsWideTile)
//        {
//            ShellTile.Create(new Uri(url, UriKind.Relative), tileData, supportsWideTile);
//        }
//
//        public bool TileExists(string url)
//        {
//            ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault(o => o.NavigationUri.ToString().Contains(url));
//            return tile == null ? false : true;
//        }
//
//        public void DeleteTile(string url)
//        {
//            ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault(o => o.NavigationUri.ToString().Contains(url));
//            if (tile == null) return;
//
//            tile.Delete();
//        }
//    }
//}
