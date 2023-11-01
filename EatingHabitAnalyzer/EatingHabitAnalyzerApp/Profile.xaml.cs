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
            await DisplayAlert("Error", "You are not logged in", "OK");
            await Shell.Current.GoToAsync($"login");
        }
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
        
        //TODO Load in profile values

        var apiResult = await _client.GetAsync(@$"https://eatinghabitanalyzer.azurewebsites.net/Profile/GetProfile");

        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Flyout);
        base.OnNavigatedTo(args);
    }
}