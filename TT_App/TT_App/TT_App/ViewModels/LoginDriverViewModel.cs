using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TT_App.Models;
using TT_App.Services;
using TT_App.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TT_App.ViewModels
{
    public class LoginDriverViewModel : BaseViewModel
    {
        public Command LoginAsDriverCommand { get; }
        public LoginDriverViewModel()
        {
            driverService = new DriverService();
            LoginAsDriverCommand = new Command(async () => await LoginDriverAsync(), () => !IsBusy);
        }

        private string textUsername;
        public string TextUsername
        {
            get
            {
                return textUsername;
            }
            set
            {
                textUsername = value;
                OnPropertyChanged();
                ShowMessageIsNotValid("", false);
            }
        }

        private string textPassword;
        public string TextPassword
        {
            get
            {
                return textPassword;
            }
            set
            {
                textPassword = value;
                OnPropertyChanged();
                ShowMessageIsNotValid("", false);
            }
        }

        private string textNotValid;
        public string TextNotValid
        {
            get { return textNotValid; }
            set
            {
                textNotValid = value;
                OnPropertyChanged();
            }
        }

        private bool showTextIsNotValid;
        public bool ShowTextIsNotValid
        {
            get { return showTextIsNotValid; }
            set
            {
                showTextIsNotValid = value;
                OnPropertyChanged();
            }
        }

        public async Task LoginDriverAsync()
        {
            IsBusy = true;
            try
            {
                var regex = new Regex(pattern: @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                var regexPass = new Regex(pattern: @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$");

                if (string.IsNullOrEmpty(TextUsername) || string.IsNullOrEmpty(TextPassword))
                {
                    ShowMessageIsNotValid("يرجى ملئ الحقول الفارغة *", true);
                    return;
                }

                if (!regex.IsMatch(TextUsername.Trim()))
                {
                    ShowMessageIsNotValid("اسم المستخدم غير صحيح *", true);
                    return;
                }

                if (!regexPass.IsMatch(TextPassword.Trim()))
                {
                    ShowMessageIsNotValid("ادخل احرف كبيرة وصغيرة ورموز واكثر من 8 احرف *", true);
                    return;
                }

                Driver DriverInfo = await driverService.GetDriverByEmailAndPasswordAsync(TextUsername, TextPassword);
                if (DriverInfo == null)
                {
                    ShowMessageIsNotValid("اسم المستخدم او كلمة المرور غير صحيحة *", true);
                    return;
                }
                var JsonDriverInfo = JsonConvert.SerializeObject(DriverInfo);
                Preferences.Set("DriverInfo", JsonDriverInfo);
                Preferences.Set("IsLogedinAsDriver", true);

                App.Current.MainPage = new NavigationPage(new StartTripsPage());
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
        public void ShowMessageIsNotValid(string message, bool isVisible)
        {
            TextNotValid = message;
            ShowTextIsNotValid = isVisible;
        }
        public override void CommandStateChanged()
        {
            LoginAsDriverCommand?.ChangeCanExecute();
        }
    }
}
