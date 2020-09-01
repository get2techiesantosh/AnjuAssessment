using Xamarin.Forms;

namespace AnjuAssessmentApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            webView.Uri = "index.html";
            webView.RegisterAction((data) =>
            {
                label.TextColor = new Color(255, 0, 0);
                label.FontSize = 30;
                label.Text = "YOU DID IT !";
            });
        }
    }
}
