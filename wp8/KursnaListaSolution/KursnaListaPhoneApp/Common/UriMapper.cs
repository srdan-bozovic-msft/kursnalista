using System;
using System.Text.RegularExpressions;
using System.Windows.Navigation;

namespace KursnaListaPhoneApp.Common
{
    public class UriMapper : UriMapperBase
    {
        public override Uri MapUri(Uri uri)
        {
            var regex = new Regex(@"/Protocol\?encodedLaunchUri=kursnalista%3AConverter%3Ffrom%3D([A-Z]+)%26to%3D([A-Z]+)");
            var match = regex.Match(uri.OriginalString);
            if (match.Success)
            {
                var from = match.Groups[1].Value;
                var to = match.Groups[2].Value;
                return new Uri(string.Format("/Views/ConverterPageView.xaml?from={0}&to={1}", from, to), UriKind.Relative);
            }
            return uri;
        }
    }
}
