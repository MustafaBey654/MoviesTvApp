using CommunityToolkit.Mvvm.Messaging.Messages;
using MoviesTvApp.Services;
using Mopups.Pages;
using Mopups.Services;
using CommunityToolkit.Mvvm.Messaging;

namespace MoviesTvApp.Pages;

public partial class FullScreenPage : PopupPage
{
    private readonly DeviceOrientationService deviceOrientationService;
    public CurrentVideoState Video { get; set; }
    public FullScreenPage(CurrentVideoState currentVideo)
	{
        Video = currentVideo;
        InitializeComponent();
        deviceOrientationService = new DeviceOrientationService();
        deviceOrientationService.SetDeviceOrientation(displayOrientation: DisplayOrientation.Landscape);
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();
        mediaElement.Source = Video.VideoUri;
        mediaElement.SeekTo(Video.Position);
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        mediaElement.Source = null;
        WeakReferenceMessenger.Default.Send(new NotifyFullScreenClosed(true));
        await MopupService.Instance.PopAsync();
    }

    private void btnChangeAspect_Clicked(object sender, EventArgs e)
    {
        if (mediaElement.Aspect == Aspect.AspectFit)
            MainThread.BeginInvokeOnMainThread(() => mediaElement.Aspect = Aspect.Fill);
        else if (mediaElement.Aspect == Aspect.Fill)
            MainThread.BeginInvokeOnMainThread(() => mediaElement.Aspect = Aspect.Center);
        else if (mediaElement.Aspect == Aspect.Center)
            MainThread.BeginInvokeOnMainThread(() => mediaElement.Aspect = Aspect.AspectFit);
    }

    private void btnChangeOrientation_Clicked(object sender, EventArgs e)
    {
        switch (DeviceDisplay.Current.MainDisplayInfo.Orientation)
        {
            case DisplayOrientation.Landscape: deviceOrientationService.SetDeviceOrientation(DisplayOrientation.Portrait); break;
            case DisplayOrientation.Portrait: deviceOrientationService.SetDeviceOrientation(DisplayOrientation.Landscape); break;
        }
    }

    void VideoPage_Unloaded(object sender, EventArgs e)
    {
        // Stop and cleanup MediaElement when we navigate away
        mediaElement.Handler?.DisconnectHandler();
    }

}

public class CurrentVideoState
{
    public Uri? VideoUri { get; set; }
    public TimeSpan Position { get; set; }
}

public class NotifyFullScreenClosed : ValueChangedMessage<bool>
{
    public NotifyFullScreenClosed(bool value) : base(value) { }
}