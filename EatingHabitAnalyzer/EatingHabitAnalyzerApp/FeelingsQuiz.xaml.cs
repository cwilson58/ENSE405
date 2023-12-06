using EatingHabitAnalyzerApp.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace EatingHabitAnalyzerApp;

public partial class FeelingsQuiz : ContentPage
{
    private readonly HttpClient _client;

    private int Q1Answer = 0;

    private int Q2Answer = 0;

    private int Q3Answer = 0;

    private DateTime Date;

    public FeelingsQuiz(DateTime date)
	{
        Date = date;
        _client = new HttpClient();
		InitializeComponent();
        Q4.IsReadOnly = false;
    }

    public FeelingsQuiz(DateTime date, int q1, int q2, int q3, string q4)
    {
        Date = date;
        Q1Answer = q1;
        Q2Answer = q2;
        Q3Answer = q3;
        _client = new HttpClient();
        InitializeComponent();

        Q4.Text = q4;
        Q4.IsReadOnly = true;

        SubmitBtn.IsEnabled = false;
        
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        switch (Q1Answer)
        {
            case 1:
                Q1_1.IsChecked = true;
                break;
            case 2:
                Q1_2.IsChecked = true;
                break;
            case 3:
                Q1_3.IsChecked = true;
                break;
            case 4:
                Q1_4.IsChecked = true;
                break;
            case 5:
                Q1_5.IsChecked = true;
                break;
        }

        switch (Q2Answer)
        {
            case 1:
                Q2_1.IsChecked = true;
                break;
            case 2:
                Q2_2.IsChecked = true;
                break;
            case 3:
                Q2_3.IsChecked = true;
                break;
            case 4:
                Q2_4.IsChecked = true;
                break;
            case 5:
                Q2_5.IsChecked = true;
                break;
        }

        switch (Q3Answer)
        {
            case 1:
                Q3_1.IsChecked = true;
                break;
            case 2:
                Q3_2.IsChecked = true;
                break;
            case 3:
                Q3_3.IsChecked = true;
                break;
            case 4:
                Q3_4.IsChecked = true;
                break;
            case 5:
                Q3_5.IsChecked = true;
                break;
        }
        base.OnNavigatedTo(args);
    }

    private async void SubmitBtn_Clicked(object sender, EventArgs e)
    {
        var token = await SecureStorage.GetAsync("jwt_token");
        if (token == null || token == default)
        {
            await DisplayAlert("Error", "You are not logged in", "OK");
            await Shell.Current.GoToAsync($"login");
            return;
        }
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var userId = await SecureStorage.GetAsync("userId");
        if (userId == null)
        {
            var apiResult = await _client.GetAsync(@$"https://eatinghabitanalyzer.azurewebsites.net/Profile/GetProfile");
            var userInfo = JsonSerializer.Deserialize<UserProfile>(await apiResult.Content.ReadAsStringAsync());    
            await SecureStorage.SetAsync("userId", userInfo.UserId.ToString());
            userId = userInfo.UserId.ToString();
        }

        var feelings = new FeelingSurvey
        {
            UserID = int.Parse(userId),
            Blurb = Q4.Text == null ? "" : Q4.Text,
            SurveyDate = new DateTime(Date.Date.Ticks,DateTimeKind.Utc),
            Q1 = Q1Answer,
            Q2 = Q2Answer,
            Q3 = Q3Answer
        };

       

        var body = JsonSerializer.Serialize(feelings);
        var result = await _client.PostAsync("https://eatinghabitanalyzer.azurewebsites.net/Tracking/AddFeelingsSurvey", new StringContent(body,Encoding.UTF8,MediaTypeHeaderValue.Parse("application/json")));

        if(!result.IsSuccessStatusCode)
        {
            await DisplayAlert("Error", $"There was an error saving the feelings survey {result.StatusCode}", "OK");
        }
        else
        {
            await Navigation.PopAsync();
        }
    }

    private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if((sender as RadioButton).IsChecked == true)
        {
            Q1Answer = int.Parse((sender as RadioButton).Value.ToString());
        }
    }

    private void RadioButton_CheckedChanged_1(object sender, CheckedChangedEventArgs e)
    {
        if ((sender as RadioButton).IsChecked == true)
        {
            Q2Answer = int.Parse((sender as RadioButton).Value.ToString());
        }
    }

    private void RadioButton_CheckedChanged_2(object sender, CheckedChangedEventArgs e)
    {
        if ((sender as RadioButton).IsChecked == true)
        {
            Q3Answer = int.Parse((sender as RadioButton).Value.ToString());
        }
    }
}