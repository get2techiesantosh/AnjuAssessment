
using AnjuAssessmentApp;
using AnjuAssessmentApp.iOS;
using Foundation;
using System.IO;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(GenericWebView), typeof(WebViewRendeEngine))]
namespace AnjuAssessmentApp.iOS
{
    public class WebViewRendeEngine : WkWebViewRenderer, IWKScriptMessageHandler
    {

        const string UIFunction = "function invokeBackEndAction(data){window.webkit.messageHandlers.invokeAction.postMessage(data);}";
        WKUserContentController userController;

        public WebViewRendeEngine() : this(new WKWebViewConfiguration())
        {
        }

        public WebViewRendeEngine(WKWebViewConfiguration config) : base(config)
        {
            userController = config.UserContentController;
            var script = new WKUserScript(new NSString(UIFunction), WKUserScriptInjectionTime.AtDocumentEnd, false);
            userController.AddUserScript(script);
            userController.AddScriptMessageHandler(this, "invokeAction");
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                userController.RemoveAllUserScripts();
                userController.RemoveScriptMessageHandler("invokeAction");
                GenericWebView genericWebView = e.OldElement as GenericWebView;
                genericWebView.Cleanup();
            }

            if (e.NewElement != null)
            {
                string filename = Path.Combine(NSBundle.MainBundle.BundlePath, $"WebPages/{((GenericWebView)Element).Uri}");
                LoadRequest(new NSUrlRequest(new NSUrl(filename, false)));
            }
        }

        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            ((GenericWebView)Element).InvokeAction(message.Body.ToString());
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