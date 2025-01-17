using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Controls;
using System.Application.UI.ViewModels;
using System.Application.UI.Views.Controls;
using System.IO;
using WinAuth;

namespace System.Application.UI.Views.Windows
{
    public class AuthTradeWindow : FluentWindow<AuthTradeWindowViewModel>
    {
        readonly ContentDialog htmlDialog;
        readonly WebView2Compat webview;

        public AuthTradeWindow() : base()
        {
            InitializeComponent();

            htmlDialog = this.FindControl<ContentDialog>("HtmlDialog");
            webview = this.FindControl<WebView2Compat>("webview");

            //htmlDialog.Opening += (s, e) =>
            //{
            //    webview.IsVisible = true;
            //};
            //htmlDialog.Closing += (s, e) =>
            //{
            //    webview.IsVisible = false;
            //};
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public async void ShowHtmlDialog_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (htmlDialog != null && ViewModel != null)
            {
                var auth = (sender as Control)?.Tag as WinAuthSteamClient.Confirmation;
                var html = ViewModel.GetConfirmationDetailHtml(auth!);

                if (OperatingSystem2.IsWindows())
                {
                    webview.HtmlSource = html;
                    webview.IsVisible = true;
                    await htmlDialog.ShowAsync();
                    webview.IsVisible = false;
                }
                else
                {
                    var path = Path.Combine(IOPath.CacheDirectory, "confirmation.html");
                    File.WriteAllText(path, html);
                    Browser2.Open($"file:///{path}");
                }
            }
        }
    }
}
