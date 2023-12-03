using OxyPlot.Maui.Skia;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;


namespace EatingHabitAnalyzerApp;

public partial class DataVisualizer : ContentPage
{
    private PlotModel _plotModel = new PlotModel { Title = "Net Calories Per Day" };

    public DataVisualizer()
	{
        InitializeComponent();
        datePickerStart.Date = DateTime.Now.AddDays(-2);
        datePickerEnd.Date = DateTime.Now.AddDays(2);
        _plotModel.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Title = "Date" });
        _plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Calories",Minimum = 0 });
    }

	protected async override void OnNavigatedTo(NavigatedToEventArgs args)
	{
        var token = await SecureStorage.GetAsync("jwt_token");
        if (token == null || token == default)
		{
            await DisplayAlert("Error", "You are not logged in", "OK");
            await Shell.Current.GoToAsync($"login");
        }

        var series1 = new LineSeries { Title = "Net Calories" };
        series1.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddDays(-2)), 1000));
        series1.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddDays(-1)), 1800));
        series1.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now), 2000));
        series1.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddDays(1)), 3000));
        series1.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddDays(1)), 3000));
        _plotModel.Series.Add(series1);

        var series2 = new LineSeries { Title = "Exercise Calories" };
        series2.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddDays(-2)), 100));
        series2.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddDays(-1)), 180));
        series2.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now), 200));
        series2.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddDays(1)), 300));
        series2.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddDays(1)), 500));
        _plotModel.Series.Add(series2);

        var plotView = new PlotView
        {
            Model = _plotModel,
            HorizontalOptions = LayoutOptions.Fill,
            HeightRequest = 400
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
}