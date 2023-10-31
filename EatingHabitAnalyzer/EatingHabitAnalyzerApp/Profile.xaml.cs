using System.Net.Http.Headers;

namespace EatingHabitAnalyzerApp;

public partial class Profile : ContentPage
{
    private HttpClient _client;

	public Profile()
	{
        _client = new HttpClient();
		InitializeComponent();
	}

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {

        var token = await SecureStorage.GetAsync("jwt_token");
        if(token == null || token == default)
        {
            await Shell.Current.GoToAsync($"login");
        }
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
        
        //TODO Load in profile values

        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Flyout);
        base.OnNavigatedTo(args);
    }
}