using System.Net.Http.Headers;
using System.Text.Json;
using EatingHabitAnalyzerApp.Models;

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
        
        var apiResult = await _client.GetAsync(@$"https://eatinghabitanalyzer.azurewebsites.net/Profile/GetProfile");
        var userInfo = JsonSerializer.Deserialize<UserProfile>(await apiResult.Content.ReadAsStringAsync());
        
        NameEntry.Text = userInfo.Name;
        EmailEntry.Text = userInfo.Email;
        HeightEntry.Text = userInfo.HeightInInches.ToString();
        WeightEntry.Text = userInfo.WeightInPounds.ToString();
        DOBEntry.Date = userInfo.DateOfBirth;
        CalsGoalEntry.Text = userInfo.GoalDailyCalories.ToString();
        GoalWeightEntry.Text = userInfo.GoalWeight.ToString();

        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Flyout);
        base.OnNavigatedTo(args);
    }
}