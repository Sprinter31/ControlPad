using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using Wpf.Ui.Controls;

namespace ControlPad
{
    public partial class EnterWebsitePopup : FluentWindow
    {
        private static readonly Regex _simpleUrlRegex = new(@"^(https?://)?([\w\-]+\.)+[\w\-]+(:\d+)?(/.*)?$", RegexOptions.IgnoreCase);
        public string? URL;
        public string? DisplayURL;
        public EnterWebsitePopup()
        {
            InitializeComponent();                        
        }

        private void btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            DisplayURL = tb_WebsiteURL.Text.Trim();
            DialogResult = true;
        }

        private void tb_WebsiteURL_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var text = tb_WebsiteURL.Text.Trim();

            if (IsValidUrl(text, out var normalized))
            {
                URL = normalized;
                btn_Ok.IsEnabled = true;
            }
            else
            {
                btn_Ok.IsEnabled = false;
            }
        }

        private static bool IsValidUrl(string input, out string normalized)
        {
            normalized = NormalizeUrl(input);
            if (!_simpleUrlRegex.IsMatch(normalized))
                return false;

            // Try System.Uri parsing for stronger validation
            return Uri.TryCreate(normalized, UriKind.Absolute, out var uri)
                   && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }

        private static string NormalizeUrl(string input)
        {
            if (!input.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !input.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return "https://" + input;
            }
            return input;
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
