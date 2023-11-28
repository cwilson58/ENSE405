using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text.Json;
using EatingHabitAnalyzerApp.Models;

namespace EatingHabitAnalyzerApp;

public partial class GroupsPage : ContentPage
{
	private HttpClient _client;

    public ObservableCollection<Group> Groups;
	public GroupsPage()
	{
		_client = new HttpClient();
        Groups = new ObservableCollection<Group>();
        InitializeComponent();
        GroupsCollection.ItemsSource = Groups;
	}
    protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("jwt_token"));

        var token = await SecureStorage.GetAsync("jwt_token");
        if (token == null || token == default)
        {
            await DisplayAlert("Error", "You are not logged in", "OK");
            await Shell.Current.GoToAsync($"login");
        }


        await LoadGroupData();

        base.OnNavigatedTo(args);
    }

    private async Task LoadGroupData()
    {
        Groups.Clear();
        var apiResult = await _client.GetAsync(@$"https://eatinghabitanalyzer.azurewebsites.net/Group/GetUserGroups");
        //instead of this use a list view, and if its empty display a message that is already in the tree but invisible.
        if (apiResult.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            EmptyLabel.IsVisible = true;
            return;
        }

        EmptyLabel.IsVisible = false;
        var rawApi = await apiResult.Content.ReadAsStringAsync();
        var groups = JsonSerializer.Deserialize<List<Group>>(rawApi, options: new JsonSerializerOptions {PropertyNameCaseInsensitive = true});
        groups.ForEach(Groups.Add);
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var btn = (Button)sender;
        var groupId = Convert.ToInt32(btn.CommandParameter);
        await Navigation.PushAsync(new GoalModal(groupId));
    }

    private void JoinButton_Clicked(object sender, EventArgs e)
    {

    }

    private void CreateButton_Clicked(object sender, EventArgs e)
    {

    }
}