using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TT_App.Models;
using TT_App.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TT_App.ViewModels
{
    public class NavUserViewModel : BaseViewModel
    {
        private User UserInfo;
        public Command ShowUserInfoCommand { get; }
        public Command<string> GoToPagesCommand { get; }
        public NavUserViewModel()
        {

            ShowUserInfoCommand = new Command(() => ShowUserInformation());
            ShowUserInfoCommand.Execute(null);

            GoToPagesCommand = new Command<string>(async (nameTap) => await GotoPageAsync(nameTap), (s) => !IsBusy);
        }

        private string personName = string.Empty;
        public string PersonName
        {
            get { return personName; }
            set
            {
                personName = value;
                OnPropertyChanged();
            }
        }

        private string email = string.Empty;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }

        public void ShowUserInformation()
        {
            try
            {
                UserInfo = JsonConvert.DeserializeObject<User>(Preferences.Get("UserInfo", null));
                if (UserInfo != null)
                {
                    PersonName = UserInfo.firstName + " " + UserInfo.lastName;
                    Email = UserInfo.email;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task GotoPageAsync(string nameTap)
        {
            try
            {
                IsBusy = true;
                switch (nameTap)
                {
                    case "Profile":
                        await App.Current.MainPage.Navigation.PushAsync(new ProfilePage());
                        break;
                    case "Stations":
                        await App.Current.MainPage.Navigation.PushAsync(new StationsPage());
                        break;
                    case "Lines":
                        await App.Current.MainPage.Navigation.PushAsync(new LinesListPage());
                        break;
                    case "Notifications":
                        await App.Current.MainPage.Navigation.PushAsync(new NotificationsPage());
                        break;
                    case "UsersGuide":
                        await App.Current.MainPage.Navigation.PushAsync(new UsersGuidePage());
                        break;
                    case "Settings":
                        await App.Current.MainPage.Navigation.PushAsync(new SettingPage());
                        break;
                    case "About":
                        await App.Current.MainPage.Navigation.PushAsync(new AboutPage());
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }
        public override void CommandStateChanged()
        {
            GoToPagesCommand?.ChangeCanExecute();
        }
    }
}
