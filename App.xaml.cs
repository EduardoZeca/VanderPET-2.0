using VanderPET_2._0.Views;
namespace VanderPET_2._0
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new frmLogin());
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);
            window.Width = 375;
            window.Height = 812;
            return window;
        }
    }
}