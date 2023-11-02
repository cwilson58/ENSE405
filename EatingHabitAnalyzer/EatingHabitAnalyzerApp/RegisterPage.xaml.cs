using EatingHabitAnalyzerApp.Models;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace EatingHabitAnalyzerApp;

public partial class RegisterPage : ContentPage
{
    private HttpClient _client;
	public RegisterPage()
	{
        _client = new HttpClient();
        InitializeComponent();
	}

    private async void SubmitButton_Clicked(object sender, EventArgs e)
    {
        var isValid = true;
        var errors = new StringBuilder();
        WaitingForServer.IsRunning = true;
        if(EmailInput.Text == "" || EmailInput.Text != EmailConfirmInput.Text)
        {
            errors.AppendLine("Email not provided or Emails do not match");
            isValid = false;
        }

        if(NameInput.Text == "")
        {
            errors.AppendLine("Name not provided");
            isValid = false;
        }

        if (Height.Text == "")
        {
            errors.AppendLine("Height not provided");
            isValid = false;
        }

        if (Weight.Text == "")
        {
            errors.AppendLine("Weight not provided");
            isValid = false;
        }

        if (GoalWeight.Text == "")
        {
            errors.AppendLine("Goal Weight not provided");
            isValid = false;
        }

        if (GoalCals.Text == "")
        {
            errors.AppendLine("Goal Calories not provided");
            isValid = false;
        }

        if(PinInput.Text.Length != 4 || PinInput.Text != PinConfirmInput.Text)
        {
            errors.AppendLine("Pin not equal to 4 digits or Pins do not match");
            isValid = false;
        }

        if(DateTime.Now - DateOfBirthInput.Date < new TimeSpan(15 * 365,0,0,0))
        {
            errors.AppendLine("You must be at least 15 years old to use this app");
            isValid = false;
        }

        var user = new UserProfile
        {
            Email = EmailInput.Text,
            Name = NameInput.Text,
            DateOfBirth = DateOfBirthInput.Date,
            HeightInInches = Convert.ToInt32(Height.Text),
            WeightInPounds = Convert.ToDouble(Weight.Text),
            GoalWeight = Convert.ToDouble(GoalWeight.Text),
            GoalDailyCalories = Convert.ToInt32(GoalCals.Text),
            Pin = PinInput.Text
        };
        if (!isValid)
        {
            WaitingForServer.IsRunning = false;
            await DisplayAlert("Error", errors.ToString(), "OK");
            return;
        }

        var jsonString = JsonSerializer.Serialize(user);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
        var result = _client.PostAsync(@"https://eatinghabitanalyzer.azurewebsites.net/Register", content).GetAwaiter().GetResult();
        WaitingForServer.IsRunning = false;
        if (result.IsSuccessStatusCode)
        {
            await DisplayAlert("Success", "You have successfully registered", "OK");
            await Shell.Current.GoToAsync($"login");
        }
        else
        {
            await DisplayAlert("Error", "An error occured while registering " + result.StatusCode, "OK");
        }
    }
}