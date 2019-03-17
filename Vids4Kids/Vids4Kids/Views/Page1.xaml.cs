using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Vids4Kids.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Page1 : ContentPage
	{
        public Page1(Models.YouTubeItem item)
        {
            InitializeComponent();

            VideoPlayer.Source = YouTubeVideoIdExtension.Convert(item.VideoId);
            //VideoPlayer.Source = item.VideoUrl;
        }
    }
}