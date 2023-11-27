namespace EatingHabitAnalyzerApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("login", typeof(LoginPage));
        Routing.RegisterRoute("main",typeof(MainPage));
        Routing.RegisterRoute("profile", typeof(Profile));
        Routing.RegisterRoute("register", typeof(RegisterPage));
        Routing.RegisterRoute("groups", typeof(GroupsPage));
    }
}