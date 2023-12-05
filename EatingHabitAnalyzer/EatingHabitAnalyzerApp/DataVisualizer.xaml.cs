using OxyPlot.Maui.Skia;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using EatingHabitAnalyzerApp.Models;
using System.Text.Json;
using System.Net.Http.Headers;
using OxyPlot.Legends;


namespace EatingHabitAnalyzerApp;

public partial class DataVisualizer : ContentPage
{
    private PlotModel _plotModel = new PlotModel { Title = "Net Calories Per Day" };

    private HttpClient _client = new HttpClient();

    public DataVisualizer()
	{
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SecureStorage.GetAsync("jwt_token").GetAwaiter().GetResult());
        InitializeComponent();
        datePickerStart.Date = DateTime.Now.AddDays(-7);
        datePickerEnd.Date = DateTime.Now;
        _plotModel.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Title = "Date" });
        _plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Calories"});
    }

	protected async override void OnNavigatedTo(NavigatedToEventArgs args)
	{
        var token = await SecureStorage.GetAsync("jwt_token");
        if (token == null || token == default)
		{
            await DisplayAlert("Error", "You are not logged in", "OK");
            await Shell.Current.GoToAsync($"login");
        }

        var rawApi = await _client.GetAsync($"https://eatinghabitanalyzer.azurewebsites.net/Tracking/GetFoodEatenPerDayInRange?start={datePickerStart.Date:yyyy/MM/dd}&end={datePickerEnd.Date:yyyy/MM/dd}");
        if (!rawApi.IsSuccessStatusCode)
        {
            await DisplayAlert("Error", "There was an error getting the data","Ok");
            return;
        }
        var points = JsonSerializer.Deserialize<List<GraphPoints>>(await rawApi.Content.ReadAsStringAsync(),new JsonSerializerOptions() { PropertyNameCaseInsensitive = true } );
        var series1 = new LineSeries { Title = "Net Calories" };
        var series2 = new LineSeries { Title = "Exercise Calories" };
        var series3 = new LineSeries { Title = "Calories Eaten" };

        foreach (var point in points)
        {
            series1.Points.Add(new DataPoint(DateTimeAxis.ToDouble(point.Date), point.CaloriesEaten - point.CaloriesBurned));
            series2.Points.Add(new DataPoint(DateTimeAxis.ToDouble(point.Date), point.CaloriesBurned));
            series3.Points.Add(new DataPoint(DateTimeAxis.ToDouble(point.Date), point.CaloriesEaten));
        }

        _plotModel.Series.Add(series1);
        _plotModel.Series.Add(series2);
        _plotModel.Series.Add(series3);
        _plotModel.Legends.Add(new Legend { LegendPlacement = LegendPlacement.Outside, LegendPosition = LegendPosition.BottomCenter, LegendOrientation = LegendOrientation.Vertical });

        var plotView = new PlotView
        {
            Model = _plotModel,
            HorizontalOptions = LayoutOptions.Fill,
            HeightRequest = 400,
        };

        PlotLayout.Children.Add(plotView);
        base.OnNavigatedTo(args);
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        _plotModel.Series.Clear();
        PlotLayout.Children.Clear();
        base.OnNavigatedFrom(args);
    }

    private async void PlotButton_Clicked(object sender, EventArgs e)
    {
        PlotLayout.Children.Clear();
        _plotModel.Series.Clear();

        var rawApi = await _client.GetAsync($"https://eatinghabitanalyzer.azurewebsites.net/Tracking/GetFoodEatenPerDayInRange?start={datePickerStart.Date:yyyy/MM/dd}&end={datePickerEnd.Date:yyyy/MM/dd}");
        if (!rawApi.IsSuccessStatusCode)
        {
            await DisplayAlert("Error", "There was an error getting the data", "Ok");
            return;
        }
        var points = JsonSerializer.Deserialize<List<GraphPoints>>(await rawApi.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        var series1 = new LineSeries { Title = "Net Calories" };
        var series2 = new LineSeries { Title = "Exercise Calories" };
        var series3 = new LineSeries { Title = "Calories Eaten" };

        foreach (var point in points)
        {
            series1.Points.Add(new DataPoint(DateTimeAxis.ToDouble(point.Date), point.CaloriesEaten - point.CaloriesBurned));
            series2.Points.Add(new DataPoint(DateTimeAxis.ToDouble(point.Date), point.CaloriesBurned));
            series3.Points.Add(new DataPoint(DateTimeAxis.ToDouble(point.Date), point.CaloriesEaten));
        }

        _plotModel.Series.Add(series1);
        _plotModel.Series.Add(series2);
        _plotModel.Series.Add(series3);
        _plotModel.Legends.Add(new Legend { LegendPlacement = LegendPlacement.Outside, LegendPosition = LegendPosition.BottomCenter, LegendOrientation = LegendOrientation.Vertical });

        var plotView = new PlotView
        {
            Model = _plotModel,
            HorizontalOptions = LayoutOptions.Fill,
            HeightRequest = 400,
        };

        PlotLayout.Children.Add(plotView);
    }
}