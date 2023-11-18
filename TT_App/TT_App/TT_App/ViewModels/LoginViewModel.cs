using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TT_App.Models;
using TT_App.Services;
using TT_App.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TT_App.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command ForgotPasswordCommand { get; }
        public Command GoToSignUpCommand { get; }
        public Command LoginCommand { get; }
        public LoginViewModel()
        {
            usersService = new UsersService();
            ForgotPasswordCommand = new Command(async () => await GoToForgotPasswordPageAsync(), () => !IsBusy);
            LoginCommand = new Command(async () => await Login(), () => !IsBusy);
            GoToSignUpCommand = new Command(async () => await GoToSignUpPageAsync(), () => !IsBusy);
            HeightFrameLogin = 130;

        }
        public string TextForBtnSignInWithGoogle { get; set; }

        private int heightFrameLogin;
        public int HeightFrameLogin
        {
            get
            {
                return heightFrameLogin;
            }
            set
            {
                heightFrameLogin = value;
                OnPropertyChanged();
            }
        }

        private bool isCliked;
        public bool IsCliked
        {
            get { return isCliked; }
            set
            {
                isCliked = value;
                OnPropertyChanged();
            }
        }

        private string textUsername;

        public string TextUsername
        {
            get { return textUsername; }
            set
            {
                textUsername = value;
                OnPropertyChanged();
                ShowMessageIsNotValid("", false);
                HeightFrameLogin = 130;

            }
        }

        private string textPassword;

        public string TextPassword
        {
            get { return textPassword; }
            set
            {

                textPassword = value;
                OnPropertyChanged();
                ShowMessageIsNotValid("", false);
                HeightFrameLogin = 130;

            }
        }

        private string textNotValid;
        public string TextNotValid
        {
            get { return textNotValid; }
            set { textNotValid = value; }
        }

        private bool showTextIsNotValid;
        public bool ShowTextIsNotValid
        {
            get { return showTextIsNotValid; }
            set
            {
                showTextIsNotValid = value;

            }
        }
        private async Task GoToForgotPasswordPageAsync()
        {
            try
            {
                IsBusy = true;
                await App.Current.MainPage.Navigation.PushAsync(new ForgotPasswordPage());
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
        private async Task GoToSignUpPageAsync()
        {
            try
            {
                IsBusy = true;
                await App.Current.MainPage.Navigation.PushAsync(new SignUpPage());
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
        private async Task Login()
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

                User UserInfo = await usersService.GetAccountByEmailAndPasswordAsync(TextUsername, TextPassword);

                if (UserInfo == null)
                {
                    ShowMessageIsNotValid("اسم المستخدم او كلمة المرور غير صحيحة *", true);
                    return;
                }

                var JsonUserInfo = JsonConvert.SerializeObject(UserInfo);

                Preferences.Set("UserInfo", JsonUserInfo);
                Preferences.Set("Logedin", true);

                App.Current.MainPage = new NavigationPage(new UserFlayoutPage());

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Message}");
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
            OnPropertyChanged(nameof(TextNotValid));
            ShowTextIsNotValid = isVisible;
            OnPropertyChanged(nameof(ShowTextIsNotValid));
            HeightFrameLogin = 170;
        }
        public override void CommandStateChanged()
        {
            ForgotPasswordCommand?.ChangeCanExecute();
            LoginCommand?.ChangeCanExecute();
            GoToSignUpCommand?.ChangeCanExecute();
        }
    }
}
