using TT_App.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TT_App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();


            if (Preferences.Get("Logedin", false) || Preferences.Get("Registered", false))
            {
                MainPage = new NavigationPage(new UserFlayoutPage());
            }
            else if (Preferences.Get("IsLogedinAsDriver", false))
            {
                MainPage = new NavigationPage(new StartTripsPage());
            }
            else
            {
                MainPage = new NavigationPage(new FirstPage());
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

    }
}
