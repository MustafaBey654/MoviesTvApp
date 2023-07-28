
using CommunityToolkit.Mvvm.Messaging;
using Mopups.Services;
using MoviesTvApp.Models;
using MoviesTvApp.Services;

namespace MoviesTvApp.Pages;

public partial class VideoPage : ContentPage
{
    private readonly Tv SendTv;
    public Uri MediaSource { get; set; }

    DeviceOrientationService deviceOrientationService;

    public VideoPage(Tv sendTv)
	{
        SendTv = sendTv;
        InitializeComponent();
        //mediaElement.Source = SendTv.tvUrl.ToString();

        this.BindingContext = this;

#if ANDROID
            btnFullScreen.IsVisible = true;
#elif IOS
        btnFullScreen.IsVisible = false;
#endif

        WeakReferenceMessenger.Default.Register<NotifyFullScreenClosed>(this, HandleOrientation);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        deviceOrientationService = new DeviceOrientationService();
        deviceOrientationService.SetDeviceOrientation(displayOrientation: DisplayOrientation.Portrait);

        MediaSource = await CreateMediaStream(SendTv.tvUrl);

        await MainThread.InvokeOnMainThreadAsync((Action)(() => mediaElement.Source = MediaSource));
    }

    private void HandleOrientation(object recipient, NotifyFullScreenClosed message)
    {
        deviceOrientationService = new DeviceOrientationService();
        deviceOrientationService.SetDeviceOrientation(displayOrientation: DisplayOrientation.Portrait);
    }

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();
        await Task.Run(() => MediaSource = null);
    }

    private async Task<Uri> CreateMediaStream(string VideoUrl)
    {
        return new Uri(VideoUrl);
    }

    private async void btnFullScreen_Clicked(object sender, EventArgs e)
    {
        if (MediaSource != null)
        {
            FullScreenPage page = new FullScreenPage(new CurrentVideoState
            {
                Position = mediaElement.Position,
                VideoUri = MediaSource,
            });

            await MopupService.Instance.PushAsync(page);
        }
    }

    void VideoPage_Unloaded(object? sender, EventArgs e)
    {
        // Stop and cleanup MediaElement when we navigate away
        mediaElement.Handler?.DisconnectHandler();
    }


}







   


   
   



