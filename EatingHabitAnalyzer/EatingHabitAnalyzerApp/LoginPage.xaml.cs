namespace EatingHabitAnalyzerApp;

public partial class LoginPage : ContentPage
{
	private HttpClient _client;
	public LoginPage()
	{
		_client = new HttpClient();
		InitializeComponent();
	}

    private async void LoginBtn_Clicked(object sender, EventArgs e)
    {
		//TODO SANATIZE INPUT
		var result = await _client.PostAsync(@$"https://eatinghabitanalyzer.azurewebsites.net/Login?email={EmailInput.Text}&pin={PinInput.Text}", null);
		if(result.StatusCode == System.Net.HttpStatusCode.OK)
		{
			await SecureStorage.SetAsync("jwt_token", await result.Content.ReadAsStringAsync());
			await SecureStorage.SetAsync("user_email", EmailInput.Text);
			await Shell.Current.GoToAsync($"//main");
		}
		else
		{
			await DisplayAlert("Login Failed", "Invalid Email or Pin", "Try Again");
		}
    }

	protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
		Shell.SetBackButtonBehavior(this, new BackButtonBehavior { IsEnabled = false });
		base.OnNavigatedTo(args);
	}
}