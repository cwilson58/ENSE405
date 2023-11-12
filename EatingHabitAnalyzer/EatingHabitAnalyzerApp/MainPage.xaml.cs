using CommunityToolkit.Maui.Markup;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using EatingHabitAnalyzerApp.Models;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EatingHabitAnalyzerApp;

public partial class MainPage : ContentPage
{

    bool firstLoad = true;

    private readonly HttpClient _client;

    public ObservableCollection<Meal> Meals;

    public Grid currentLayout;

    public int CalorieGoal = 0;

    public int CaloriesEaten = 0;

    public int CaloriesBurned = 0;

    public int CaloriesRemaining => CalorieGoal - CaloriesEaten + CaloriesBurned;

    public string CalsLabelText => $"Goal: {CalorieGoal} - Consumned: {CaloriesEaten} + Burned: {CaloriesBurned} = {CaloriesRemaining} calories remaining";
    public MainPage()
    {
        _client = new HttpClient();
        Meals = new ObservableCollection<Meal>();
        InitializeComponent();
        MealsContainer.ItemsSource(Meals);
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        USER_NAME.Text = await SecureStorage.GetAsync("user_email");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("jwt_token"));

        var token = await SecureStorage.GetAsync("jwt_token");
        if (token == null || token == default)
        {
            await DisplayAlert("Error", "You are not logged in", "OK");
            await Shell.Current.GoToAsync($"login");
            firstLoad = true;
        }
        if (firstLoad)
        {
            CalsLabel.Text = CalsLabelText;
            firstLoad = false;
        }
        var apiResult = await _client.GetAsync(@$"https://eatinghabitanalyzer.azurewebsites.net/Profile/GetProfile");
        var userInfo = JsonSerializer.Deserialize<UserProfile>(await apiResult.Content.ReadAsStringAsync());
        CalorieGoal = userInfo.GoalDailyCalories;
        
        await LoadMealData();



        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Flyout);
        base.OnNavigatedTo(args);
    }

    private async Task LoadMealData()
    {
        var apiResult = await _client.GetAsync(@$"https://eatinghabitanalyzer.azurewebsites.net/Tracking/GetDiary?date={DiaryDatePicker.Date:yyyy-MM-dd}");
        //instead of this use a list view, and if its empty display a message that is already in the tree but invisible.
        if (apiResult.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            EmptyLabel.IsVisible = true;
            return;
        }
       
        EmptyLabel.IsVisible = false;
        var rawApi = await apiResult.Content.ReadAsStringAsync();
        var entry = JsonSerializer.Deserialize<DiaryEntry>(rawApi);
        entry.Meals.ForEach(Meals.Add);
        CaloriesEaten = entry.Meals.Sum(x => x.TotalCalories);
        CalsLabel.Text = CalsLabelText;
    }

    protected override async void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        Meals.Clear();
        base.OnNavigatedFrom(args);
    }

    private async void LogOut_Clicked(object sender, EventArgs e)
    {
        //Remove the email and token from secure storage
        SecureStorage.Remove("user_email");
        SecureStorage.Remove("jwt_token");
        Meals.Clear();
        await Shell.Current.GoToAsync($"login");
        firstLoad = true;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        //TODO dynamically get the meal number
        var lastNumber = 0;
        if (Meals.Any())
        {
            lastNumber = Meals.Max(x => x.MealNumber);
        }
        var numRaw = await _client.PostAsync(@$"https://eatinghabitanalyzer.azurewebsites.net/Tracking/AddMeal?mealNumber={lastNumber + 1}&date={DiaryDatePicker.Date:yyyy-MM-dd}",new StringContent(""));
        if (numRaw.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            await _client.PostAsync(@$"https://eatinghabitanalyzer.azurewebsites.net/Tracking/NewDiary?date={DiaryDatePicker.Date:yyyy-MM-dd}", new StringContent(""));
            numRaw = await _client.PostAsync(@$"https://eatinghabitanalyzer.azurewebsites.net/Tracking/AddMeal?mealNumber={lastNumber + 1}&date={DiaryDatePicker.Date:yyyy-MM-dd}", new StringContent(""));
        }
        else if (!numRaw.IsSuccessStatusCode)
        {
            await DisplayAlert("Error", "An Error Occured", "OK");
            return;
        }
        var number = Convert.ToInt32(await numRaw.Content.ReadAsStringAsync());
        if(number == 0)
        {
            await DisplayAlert("Error", "An Error Occured", "OK");
            return;
        }
        await Navigation.PushAsync(new MealTrackingModal(number,lastNumber+1));
    }

    private async void DiaryDatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        Meals.Clear();
        await LoadMealData();
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        var btn = (Button)sender;
        var mealId = Convert.ToInt32(btn.CommandParameter);

        var meal = Meals.FirstOrDefault(x => x.MealId == mealId);
        
        var res = await DisplayAlert("Delete Meal", $"Are you sure you want to delete meal number {meal.MealNumber}?", "Yes", "No"); 
        if(res == false)
        {
            return;
        }

        var result = await _client.DeleteAsync(@$"https://eatinghabitanalyzer.azurewebsites.net/Tracking/DeleteMeal?mealId={mealId}");
        if(!result.IsSuccessStatusCode)
        {
            await DisplayAlert("Error", "An Error Occured", "OK");
            return;
        }

        Meals.Remove(meal);
    }

    private async void Edit_Button_Clicked(object sender, EventArgs e)
    {
        var btn = (Button)sender;
        var mealId = Convert.ToInt32(btn.CommandParameter);
        await Navigation.PushAsync(new MealTrackingModal(Meals.First(x => x.MealId == mealId)));
    }
}