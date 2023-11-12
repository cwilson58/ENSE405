using EatingHabitAnalyzerApp.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text.Json;

namespace EatingHabitAnalyzerApp;

public partial class MealTrackingModal : ContentPage
{
	private int _mealId;
	private int _mealNumber;
	public string MealLabelText => $"Meal {_mealNumber}";

	public readonly ObservableCollection<MealFood> FoodsToAdd = new ObservableCollection<MealFood>(); 

	private readonly List<Food> _foodsToRemove = new List<Food>();

	private readonly HttpClient _client = new HttpClient();
	public MealTrackingModal(int mealId, int mealNumber)
	{
		_mealId = mealId;
		_mealNumber = mealNumber;
		_client = new HttpClient();
		InitializeComponent();
		MealLabel.SetBinding(Label.TextProperty, nameof(MealLabelText));
        FoodsView.ItemsSource = FoodsToAdd;
    }

	protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	{
		_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("jwt_token"));
		base.OnNavigatedTo(args);
	}

    private async void Save_Clicked(object sender, EventArgs e)
    {
        //TODO save logic
        foreach (var food in FoodsToAdd)
        {
			await _client.PostAsync(@$"https://eatinghabitanalyzer.azurewebsites.net/Tracking/AddFood?barcode={food.Barcode}&mealId={_mealId}&numberOfServings={food.NumberOfServings}", new StringContent(""));
        }
        await Navigation.PopAsync();
    }

    private async void Add_Clicked(object sender, EventArgs e)
    {
		var input = await DisplayPromptAsync("Add Food", "Enter Barcode",accept: "Add", placeholder: "0000000000000", keyboard: Keyboard.Numeric);
		if(input.Length != 13)
		{
            input = input.PadLeft(13, '0');
        }
		var food = await _client.GetAsync(@$"https://eatinghabitanalyzer.azurewebsites.net/Food/GetFood?barcode={input}");
		if (food.IsSuccessStatusCode)
		{
			var rawString = await food.Content.ReadAsStringAsync();
            var foodToAdd = JsonSerializer.Deserialize<Food>(rawString,options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
			var servings = Convert.ToInt32(await DisplayPromptAsync("Add Food", "Enter number of servings", accept: "Add", placeholder: "1", keyboard: Keyboard.Numeric));
			var mealFoodToAdd = new MealFood
			{
				NumberOfServings = servings,
				NumberOfGrams = foodToAdd.ServingSizeInGrams * servings,
				Food = foodToAdd,
				Barcode = foodToAdd.Barcode,
				MealId = _mealId,
			};
			FoodsToAdd.Add(mealFoodToAdd);
		}
		else
		{
			await DisplayAlert("Error", "Food not found", "OK");
		}
	}
}