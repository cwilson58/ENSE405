namespace EatingHabitAnalyzerApp
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        bool firstLoad = true;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {

        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            USER_NAME.Text = await SecureStorage.GetAsync("user_email");
            if (firstLoad)
            {
                DiaryDatePicker.Date = DateTime.Today;
                firstLoad = false;
            }
            Shell.SetFlyoutBehavior(this, FlyoutBehavior.Flyout);
            base.OnNavigatedTo(args);
        }
    }
}