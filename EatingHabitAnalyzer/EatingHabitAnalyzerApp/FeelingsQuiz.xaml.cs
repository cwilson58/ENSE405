namespace EatingHabitAnalyzerApp;

public partial class FeelingsQuiz : ContentPage
{
	public FeelingsQuiz()
	{
		InitializeComponent();
	}

    private async void SubmitBtn_Clicked(object sender, EventArgs e)
    {
		await Navigation.PopAsync();
    }
}