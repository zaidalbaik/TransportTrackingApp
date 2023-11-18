using System.Threading.Tasks;
using TT_App.Views;
using Xamarin.Forms;

namespace TT_App.ViewModels
{
    public class AdminViewModel : BaseViewModel
    {
        public Command<string> NavCommand { get; }
        public AdminViewModel()
        {
            NavCommand = new Command<string>(async (PageName) => await GoToPageAsync(PageName), (s) => !IsBusy);

        }
        public async Task GoToPageAsync(string PageName)
        {
            try
            {
                IsBusy = true;
                switch (PageName.Trim())
                {
                    case "Users":
                        await App.Current.MainPage.Navigation.PushAsync(new UsersPage());

                        break;
                    case "Drivers":
                        await App.Current.MainPage.Navigation.PushAsync(new DriversListPage());

                        break;
                    case "Buses":
                        await App.Current.MainPage.Navigation.PushAsync(new BusesListPage());

                        break;
                }
            }
            catch (System.Exception)
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
            NavCommand?.ChangeCanExecute();
        }
    }
}
