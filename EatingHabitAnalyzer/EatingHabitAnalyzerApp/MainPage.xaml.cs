using CommunityToolkit.Maui.Markup;
using System.Net.Http.Headers;

namespace EatingHabitAnalyzerApp;

public partial class MainPage : ContentPage
{
    private readonly Grid _layoutTemplate;
    bool firstLoad = true;

    private readonly HttpClient _client;
    public MainPage()
    {
        _client = new HttpClient();
        _layoutTemplate = new Grid()
        {
            BackgroundColor = Colors.AntiqueWhite,
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
            }
        };
        InitializeComponent();
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        USER_NAME.Text = await SecureStorage.GetAsync("user_email");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("jwt_token"));
        if (firstLoad)
        {
            DiaryDatePicker.Date = DateTime.Today;
            firstLoad = false;
        }
        //TODO Load in diary values for selected date

        var apiResult = await _client.GetAsync(@$"https://eatinghabitanalyzer.azurewebsites.net/Tracking/GetDiary?date=""{DiaryDatePicker.Date:yyyy-MM-dd}""");

        _layoutTemplate.Add(new Label()
        {
            Text = "Meals",
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
        }.ColumnSpan(2));

        //foreach meal, complete the template and add it to the stacklayout

        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Flyout);
        base.OnNavigatedTo(args);
    }

    private async void LogOut_Clicked(object sender, EventArgs e)
    {
        //Remove the email and token from secure storage
        SecureStorage.Remove("user_email");
        SecureStorage.Remove("jwt_token");
        await Shell.Current.GoToAsync($"login");
    }
}