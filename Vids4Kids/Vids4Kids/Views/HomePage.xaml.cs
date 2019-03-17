using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Vids4Kids.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Vids4Kids.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private const string ApiKey = "AIzaSyAKs8bC1SLTI1X0_7WFaLYqRDEcthP1tVY";

        string channelUrl = "https://www.googleapis.com/youtube/v3/search?part=id&maxResults=20&channelId="
    + "UCozAuGJZ5MPoijz8IE_1Gtw"
    //+ "Your_ChannelId"
    + "&key="
    + ApiKey;


        string detailsUrl = "https://www.googleapis.com/youtube/v3/videos?part=snippet,statistics&id="
            + "{0}"
            //+ "Your_Videos_Id"
            + "&key="
            + ApiKey;

        public List<YouTubeItem> Items { get; set; } = new List<YouTubeItem>();

        public HomePage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
           await GetChannelData();
            base.OnAppearing();
        }

        async Task GetChannelData()
        {
            var videoIds = new List<string>();
            var Test = new List<YouTubeItem>();

            try
            {
                var httpClient = new HttpClient();

                var json = await httpClient.GetStringAsync(channelUrl);

                // Deserialize our data, this is in a simple List format
                var response = JsonConvert.DeserializeObject<YouTubeApiListRoot>(json);

                // Add all the video id's we've found to our list.
                videoIds.AddRange(response.items.Select(item => item.id.videoId));

                // Get the details for all our items
                Items = await GetVideoDetailsAsync(videoIds);
                listVids.ItemsSource = Items;
                foreach (var item in Items)
                {
                    var url = item.VideoUrl;
                }
            }
            catch (Exception ex)
            {
                var ms = ex;
            }
        }

        async Task<List<YouTubeItem>> GetVideoDetailsAsync(List<string> videoIds)
        {
            try
            {
                var videoIdString = string.Join(",", videoIds);
                var youtubeItems = new List<YouTubeItem>();

                var httpClient = new HttpClient();
                
                    var json = await httpClient.GetStringAsync(string.Format(detailsUrl, videoIdString));
                    var response = JsonConvert.DeserializeObject<YouTubeApiDetailsRoot>(json);

                    foreach (var item in response.items)
                    {
                    var youTubeItem = new YouTubeItem()
                    {
                            Title = item.snippet.title,
                            Description = item.snippet.description,
                            ChannelTitle = item.snippet.channelTitle,
                            PublishedAt = item.snippet.publishedAt,
                            VideoId = item.id,
                            DefaultThumbnailUrl = item.snippet?.thumbnails?.@default?.url,
                            MediumThumbnailUrl = item.snippet?.thumbnails?.medium?.url,
                            HighThumbnailUrl = item.snippet?.thumbnails?.high?.url,
                            StandardThumbnailUrl = item.snippet?.thumbnails?.standard?.url,
                            MaxResThumbnailUrl = item.snippet?.thumbnails?.maxres?.url,
                            ViewCount = item.statistics?.viewCount,
                            LikeCount = item.statistics?.likeCount,
                            DislikeCount = item.statistics?.dislikeCount,
                            FavoriteCount = item.statistics?.favoriteCount,
                            CommentCount = item.statistics?.commentCount,
                            Tags = item.snippet?.tags
                        };

                        youtubeItems.Add(youTubeItem);
                    }
                

                return youtubeItems;
            }
            catch (Exception ex)
            {
                var ms = ex;
                return new List<YouTubeItem>();
            }
        }

        private void ListVids_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var item = e.SelectedItem as YouTubeItem;

                Navigation.PushAsync(new Page1(item));

                ((ListView)sender).SelectedItem = null; // de-select the row
            }
        }
    }
}