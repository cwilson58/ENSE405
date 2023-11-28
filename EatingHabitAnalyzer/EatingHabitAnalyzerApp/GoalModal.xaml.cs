using EatingHabitAnalyzerApp.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace EatingHabitAnalyzerApp;

public partial class GoalModal : ContentPage
{
	private readonly HttpClient _client = new HttpClient();

	public int GroupId { get; set; }
	public GoalModal(int groupId)
	{
		GroupId = groupId;
        InitializeComponent();
    }

	protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	{
		_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("jwt_token"));
		base.OnNavigatedTo(args);
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		var goal = new Goal
		{
			GroupID = GroupId,
			LostPounds = LostPoundsEntry.Text == null ? 0 : int.Parse(LostPoundsEntry.Text),
			ExerciseCalories = ExerciseCalories.Text == null ? 0 : int.Parse(ExerciseCalories.Text),
			Custom = Custom.Text == null ? "" : Custom.Text,
			CompleteBy = DatePicker.Date
		};

		var json = JsonSerializer.Serialize(goal);
		var content = new StringContent(json, Encoding.UTF8, "application/json");
		var response = await _client.PostAsync("https://eatinghabitanalyzer.azurewebsites.net/Group/CreateGoal", content);

		if (response.IsSuccessStatusCode)
		{
			await Navigation.PopModalAsync();
		}
		else
		{
            await DisplayAlert("Error", $"Something went wrong {response.Content.ReadAsStringAsync().GetAwaiter().GetResult()}", "OK");
        }
    }
}