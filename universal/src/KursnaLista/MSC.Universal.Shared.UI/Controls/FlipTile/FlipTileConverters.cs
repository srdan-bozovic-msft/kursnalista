// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace MSC.Universal.Shared.UI.Controls
{
    /// <summary>
    /// Returns the Flip tile width corresponding to a tile size.
    /// </summary>
    public class TileSizeToWidthConverter : IValueConverter
    {
        /// <summary>
        /// Converts from a tile size to the corresponding width.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double baseWidth = 0;

            switch ((TileSize)value)
            {
                case TileSize.Default:
                    baseWidth = 173;
                    break;

                case TileSize.Small:
                    baseWidth = 99;
                    break;

                case TileSize.Medium:
                    baseWidth = 210;
                    break;

                case TileSize.Large:
                    baseWidth = 432;
                    break;
            }

            double multiplier;

            if (parameter == null || double.TryParse(parameter.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out multiplier) == false)
            {
                multiplier = 1;
            }

            return baseWidth * multiplier;
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Returns the Flip tile height corresponding to a tile size.
    /// </summary>
    public class TileSizeToHeightConverter : IValueConverter
    {
        /// <summary>
        /// Converts from a tile size to the corresponding height.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double baseHeight = 0;

            switch ((TileSize)value)
            {
                case TileSize.Default:
                    baseHeight = 173;
                    break;

                case TileSize.Small:
                    baseHeight = 99;
                    break;

                case TileSize.Medium:
                    baseHeight = 210;
                    break;

                case TileSize.Large:
                    baseHeight = 210;
                    break;
            }

            double multiplier;

            if (parameter == null || double.TryParse(parameter.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out multiplier) == false)
            {
                multiplier = 1;
            }

            return baseHeight * multiplier;
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
