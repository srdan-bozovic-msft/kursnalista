using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.Phone.Shared.Contracts.PhoneServices
{
    public interface ITileService
    {
        void CreateTile(string url, 
            ShellTileData tileData, bool supportsWideTile);
        bool TileExists(string url);
        void DeleteTile(string url);
    }
}
