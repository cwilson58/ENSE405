namespace EatingHabitAnalyzerApp;

public partial class MealTrackingModal : ContentPage
{
	private int _mealNumber;
	public MealTrackingModal(int mealNumber)
	{
		_mealNumber = mealNumber;
		InitializeComponent();
	}

    private async void Save_Clicked(object sender, EventArgs e)
    {
		//TODO save logic
		await Navigation.PopAsync();
    }
}