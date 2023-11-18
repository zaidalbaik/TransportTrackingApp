using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TT_App.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TT_App.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {

        public Command LogOutCommand { get; }
        public SettingsViewModel()
        {
            LogOutCommand = new Command(async () => await LogOutAsync());
        }


        public async Task LogOutAsync()
        {
            try
            {
                IsBusy = true;
                bool IsLogout = await App.Current.MainPage.DisplayAlert("تسجيل الخروج", "هل انت متأكد من تسجيل الخروج", "Ok", "Cancel");
                if (IsLogout == false)
                    return;
                if (IsLogout == true)
                {
                    App.Current.MainPage = new NavigationPage(new LoginPage());
                    Preferences.Clear();
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
            LogOutCommand?.ChangeCanExecute();
        }
    }
}
