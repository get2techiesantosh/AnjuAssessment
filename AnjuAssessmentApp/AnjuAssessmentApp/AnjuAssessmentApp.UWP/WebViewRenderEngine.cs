using AnjuAssessmentApp;
using AnjuAssessmentApp.UWP;
using System;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(GenericWebView), typeof(WebViewRenderEngine))]
namespace AnjuAssessmentApp.UWP
{
    
    public class WebViewRenderEngine : WebViewRenderer
    {

        const string UIFunction = "function invokeBackendAction(data){window.external.notify(data);}";
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {

            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                Control.NavigationCompleted -= OnWebViewNavigationCompleted;
                Control.ScriptNotify -= OnWebViewScriptNotify;
            }
            if (e.NewElement != null)
            {
                Control.NavigationCompleted += OnWebViewNavigationCompleted;
                Control.ScriptNotify += OnWebViewScriptNotify;
                Control.Source = new Uri($"ms-appx-web:///WebPages//{((GenericWebView)Element).Uri}");
            }

        }

        async void OnWebViewNavigationCompleted(Windows.UI.Xaml.Controls.WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (args.IsSuccess)
            {
                // Inject JS script
                await Control.InvokeScriptAsync("eval", new[] { UIFunction });
            }
        }

        void OnWebViewScriptNotify(object sender, NotifyEventArgs e)
        {
            ((GenericWebView)Element).InvokeAction(e.Value);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ((GenericWebView)Element).Cleanup();
            }
            base.Dispose(disposing);
        }
    }
}
