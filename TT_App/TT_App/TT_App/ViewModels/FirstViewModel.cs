using System;
using System.Threading.Tasks;
using TT_App.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TT_App.ViewModels
{
    public class FirstViewModel : BaseViewModel
    {        
        public Command<string> GotoPageCommand { get; }
         
        public FirstViewModel()
        {
            GotoPageCommand = new Command<string>(async (NamePage) => await GoToPagesAsync(NamePage), (s) => !IsBusy);
        }

        public async Task GoToPagesAsync(string NamePage)
        {
            try
            {
                IsBusy = true;
                switch (NamePage)
                {
                    case "LoginPage":
                        await App.Current.MainPage.Navigation.PushAsync(new LoginPage());
                        break;

                    case "LoginAsDriverPage":
                        await App.Current.MainPage.Navigation.PushAsync(new LoginAsDriverPage());
                        break;
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

        public double FlexWidth => DeviceDisplay.MainDisplayInfo.Width;

        public override void CommandStateChanged()
        {
            GotoPageCommand?.ChangeCanExecute();
        }

    }
}
