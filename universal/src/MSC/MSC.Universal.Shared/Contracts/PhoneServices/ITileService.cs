using System;
using Windows.Data.Xml.Dom;

namespace MSC.Universal.Shared.Contracts.PhoneServices
{
    public interface ITileService
    {
        void UpdateTile(XmlDocument tileXml, TimeSpan expiration);
    }
}
