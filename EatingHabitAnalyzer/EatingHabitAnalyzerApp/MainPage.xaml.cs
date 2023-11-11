using CommunityToolkit.Maui.Markup;
using System.Net.Http.Headers;

namespace EatingHabitAnalyzerApp;

public partial class MainPage : ContentPage
{
    private readonly Grid _layoutTemplate;
    bool firstLoad = true;

    private readonly HttpClient _client;

    public Grid currentLayout;
    public MainPage()
    {
        _client = new HttpClient();
        _layoutTemplate = new Grid()
        {
            Margin = new Thickness(0, 10, 0, 10),
            Shadow = new Shadow()
            {
                Brush = Colors.Black,
                Offset = new Point(10, 10),
                Opacity = 0.8f,
            },
            ColumnDefinitions = new ColumnDefinitionCollection()
            {
                new ColumnDefinition() {Width = GridLength.Star},
                new ColumnDefinition() {Width = GridLength.Star},
            },
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition(height: GridLength.Star),
                new RowDefinition(height: GridLength.Star)
            }
    };
        InitializeComponent();
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
            DiaryDatePicker.Date = DateTime.Today;
            firstLoad = false;
        }
        //TODO Load in diary values for selected'
        currentLayout = new Grid()
        {
            Margin = _layoutTemplate.Margin,
            Shadow = _layoutTemplate.Shadow,
            ColumnDefinitions = _layoutTemplate.ColumnDefinitions,
            RowDefinitions = _layoutTemplate.RowDefinitions
        };
        currentLayout.Add(new Label()
        {
            Text = "Meals",
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
        }.ColumnSpan(2));

        var apiResult = await _client.GetAsync(@$"https://eatinghabitanalyzer.azurewebsites.net/Tracking/GetDiary?date={DiaryDatePicker.Date:yyyy-MM-dd}");
        if (apiResult.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            currentLayout.Add(new Label() {
                Text = "Diary Has not been created, add a meal to create a diary!",
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            }.Row(2).ColumnSpan(2));
        }
        else
        {
            await DisplayAlert("Why?", $"This is: {apiResult.StatusCode}", "OK");
            //foreach meal, complete the template
            //SKIP THE FIRST ROW AS IT WILL ALREADY BE THERE 
        }

        MealsContainer.Add(currentLayout);
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Flyout);
        base.OnNavigatedTo(args);
    }

    protected override async void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        await DisplayAlert("LEAVING", "LEAVING", "OK");
        MealsContainer.Children.Clear();
        base.OnNavigatedFrom(args);
    }

    private async void LogOut_Clicked(object sender, EventArgs e)
    {
        //Remove the email and token from secure storage
        SecureStorage.Remove("user_email");
        SecureStorage.Remove("jwt_token");
        await Shell.Current.GoToAsync($"login");
        firstLoad = true;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        //TODO dynamically get the meal number
        await Navigation.PushAsync(new MealTrackingModal(0));
    }
}