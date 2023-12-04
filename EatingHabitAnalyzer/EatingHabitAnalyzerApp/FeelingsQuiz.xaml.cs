using EatingHabitAnalyzerApp.Models;
using System.Text;
using System.Text.Json;

namespace EatingHabitAnalyzerApp;

public partial class FeelingsQuiz : ContentPage
{
    private readonly HttpClient _client;

    private int Q1Answer = -1;

    private int Q2Answer = -1;

    private int Q3Answer = -1;

    public FeelingsQuiz()
	{
        _client = new HttpClient();
		InitializeComponent();
	}

    public FeelingsQuiz(int q1, int q2, int q3)
    {
        Q1Answer = q1;
        Q2Answer = q2;
        Q3Answer = q3;
        _client = new HttpClient();
        InitializeComponent();

        SubmitBtn.IsEnabled = false;
        switch (q1)
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

        switch(q2)
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

        switch (q3)
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
    }

    private async void SubmitBtn_Clicked(object sender, EventArgs e)
    {
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
            Blurb = "",
            SurveyDate = DateTime.Now,
            Q1 = Q1Answer,
            Q2 = Q2Answer,
            Q3 = Q3Answer
        };

        var result = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Post, "https://eatinghabitanalyzer.azurewebsites.net/Tracking/PostFeelingsSurvey")
        {
            Content = new StringContent(JsonSerializer.Serialize(feelings), Encoding.UTF8)
        });

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