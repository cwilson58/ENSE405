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
        else
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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
        }

        base.OnNavigatedTo(args);
    }

    private void WeightGoalButton_Clicked(object sender, EventArgs e)
    {
        GoalWeightEntry.IsEnabled = true;
    }

    private void CalGoalBtn_Clicked(object sender, EventArgs e)
    {
        CalsGoalEntry.IsEnabled = true;
    }

    //Switch this to DisplayPrompt
    private async void SaveChangesBtn_Clicked(object sender, EventArgs e)
    {
        if (GoalWeightEntry.IsEnabled)
        {
            GoalWeightEntry.IsEnabled = false;
            var result = await  _client.PatchAsync(@$"https://eatinghabitanalyzer.azurewebsites.net/Profile/SetWeightGoal?goal=""{GoalWeightEntry.Text}""",new StringContent(""));
            if (result.IsSuccessStatusCode)
            {
                await DisplayAlert("Goal Weight Changed", "Change Successfull!", "OK");
            }

            else
            {
                await DisplayAlert("Error", $"There was an error saving the goal {result.StatusCode}", "OK");
            }
        }

        if (CalsGoalEntry.IsEnabled)
        {
            CalsGoalEntry.IsEnabled = false;
            
            var result = await _client.PatchAsync(@$"https://eatinghabitanalyzer.azurewebsites.net/Profile/SetCalorieGoal?goal=""{CalsGoalEntry.Text}""", new StringContent(""));
            if (result.IsSuccessStatusCode)
            {
                await DisplayAlert("Goal Weight Changed", "Change Successfull!", "OK");
            }

            else
            {
                await DisplayAlert("Error", $"There was an error saving the goal {result.StatusCode}", "OK");
            }
        }
    }
}