using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TT_App.Models;
using TT_App.Services;
using TT_App.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TT_App.ViewModels
{
    public class StartTripViewModel : BaseViewModel
    {
        public Command GetLinesCommand { get; }
        public Command StartTripCommand { get; }

        public Command SetActivityCommand { get; }
        public Command ChangeActivityCommand { get; }

        public Command<string> GoToPagesFromDriverCommand { get; }

        public Command LogOutDriverCommand { get; }

        public List<Line> Lines { get; set; }
        public StartTripViewModel()
        {
            lineService = new LineService();
            busService = new BusService();

            SetActivityCommand = new Command(async () => await SetActivity());
            SetActivityCommand.Execute(null);

            ChangeActivityCommand = new Command(async () => await ChangeActivityOfBus());

            GetLinesCommand = new Command(async () => await FillLines());
            GetLinesCommand.Execute(null);


            StartTripCommand = new Command(async () => await StartTrip(), () => !IsBusy);

            GoToPagesFromDriverCommand = new Command<string>(async (nameTap) => await GotoPageAsync(nameTap), (s) => !IsBusy);

            LogOutDriverCommand = new Command(async () => await LogOutAsync(), () => !IsBusy);
        }
        public string DriverName => GetDriverInfo().DriverName;
        public string DriverEmail => GetDriverInfo().Email;
        public string DriverId => $"Your-Id : {GetDriverInfo().Id}";
        public string BusId => $"Bus-Id : {GetDriverInfo().BusId}";


        private bool isActive;
        public bool IsActive
        {
            get => isActive;
            set
            {
                isActive = value;
                OnPropertyChanged();

                ChangeActivityCommand.Execute(null);
            }
        }

        private Line selectedLine;
        public Line SelectedLine
        {
            get { return selectedLine; }
            set
            {
                selectedLine = value;
                OnPropertyChanged();
            }
        }
        public async Task FillLines()
        {
            try
            {
                Lines = await lineService.GetAllLinesAsync();
                OnPropertyChanged(nameof(Lines));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task SetActivity()
        {
            try
            {
                var Bus = await busService.GetBusAsync(GetDriverInfo().BusId);
                IsActive = Bus.IsActive;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Driver GetDriverInfo()
        {
            try
            {
                return JsonConvert.DeserializeObject<Driver>(Preferences.Get("DriverInfo", ""));
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task StartTrip()
        {
            try
            {
                if (SelectedLine == null)
                    return;

                IsBusy = true;

                var newBus = new Bus()
                {
                    LineId = SelectedLine.LineID,
                };

                await busService.EditBusAsync(GetDriverInfo().BusId, newBus);


                //Bus activity
                IsActive = true;

                Trip newTrip = new Trip()
                {
                    LineId = SelectedLine.LineID,
                    DriverId = GetDriverInfo().Id,
                    Departure_time = DateTime.Now
                };

                var JsonSelectedLine = JsonConvert.SerializeObject(SelectedLine);
                var JsonTripInfo = JsonConvert.SerializeObject(newTrip);

                Preferences.Set("SelectedLine", JsonSelectedLine);
                Preferences.Set("TripInfo", JsonTripInfo);

                await App.Current.MainPage.Navigation.PushAsync(new MapDriverPage());
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

        public async Task ChangeActivityOfBus()
        {
            try
            {
                var newBus = new Bus()
                {
                    IsActive = this.IsActive,
                };

                await busService.EditActivityForBus(GetDriverInfo().BusId, newBus);
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
                    case "Settings":
                        await App.Current.MainPage.Navigation.PushAsync(new SettingDriverPage());
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
                    App.Current.MainPage = new NavigationPage(new LoginAsDriverPage());
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
            GoToPagesFromDriverCommand?.ChangeCanExecute();
            LogOutDriverCommand?.ChangeCanExecute();          
            StartTripCommand?.ChangeCanExecute();
        }
    }
}
