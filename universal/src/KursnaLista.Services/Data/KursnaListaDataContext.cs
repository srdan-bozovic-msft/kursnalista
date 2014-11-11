using System;
using System.IO;
using MSC.Universal.Shared.Contracts.Services;

namespace KursnaLista.Services.Data
{
    public class KursnaListaDataContext : IDataContext
    {
        private readonly ISettingsService _settingsService;
        private string _context;

        public KursnaListaDataContext(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public string Context
        {
            get
            {
                if (_context == null)
                {
                    _context = _settingsService.Get<string>("Settings_DataSourceContext");
                }
                return _context;
            }
            set
            {
                if (_context != value)
                {
                    _settingsService.Set("Settings_DataSourceContext", value);
                    _context = value;
                    OnContextChanged();
                }
            }
        }

        public event EventHandler ContextChanged;

        protected void OnContextChanged()
        {
            if (ContextChanged != null)
            {
                ContextChanged(this, EventArgs.Empty);
            }
        }

        public string GetServerAddress()
        {
            return "https://kursna-lista.azure-mobile.net";
        }
    }
}