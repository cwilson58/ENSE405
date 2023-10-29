namespace EatingHabitAnalyzerApp;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private async void LoginBtn_Clicked(object sender, EventArgs e)
    {
		//Make request to the server for a JWT token
		//Save token in secure storage
		if(EmailInput.Text == "caw522@uregina.ca" && PinInput.Text == "0234")
		{
			await SecureStorage.SetAsync("jwt_token", "fake_jwt_token");
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
        base.OnNavigatedTo(args);
    }
}