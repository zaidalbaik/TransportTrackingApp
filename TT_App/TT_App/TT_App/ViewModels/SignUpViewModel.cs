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
    public class SignUpViewModel : BaseViewModel
    {
        public Command CreateAccountCommand { get; }
        public SignUpViewModel()
        {
            usersService = new UsersService();
            CreateAccountCommand = new Command(async () => await CreateUserAccountAsync(), () => !IsBusy);

        }
        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged();
            }
        }
        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged();
            }
        }
        private string email = null;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged();
                ShowMessageIsNotValid("", false);

            }
        }

        private string password = null;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged();
                ShowMessageIsNotValid("", false);
            }
        }

        private string confirmPassword = null;
        public string ConfirmPassword
        {
            get
            {
                return confirmPassword;
            }
            set
            {
                confirmPassword = value;
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

        public void ShowMessageIsNotValid(string message, bool isVisible)
        {
            textNotValid = message;
            OnPropertyChanged(nameof(TextNotValid));
            showTextIsNotValid = isVisible;
            OnPropertyChanged(nameof(ShowTextIsNotValid));
        }
        public User GetUserInfo()
        {
            User user = new User()
            {
                firstName = FirstName,
                lastName = LastName,
                email = Email,
                password = Password,
                confirmPassword = ConfirmPassword
            };

            return user;
        }

        //--------
        public async Task CreateUserAccountAsync()
        {
            IsBusy = true;
            try
            {
                var regexEmail = new Regex(pattern: @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                var regexPass = new Regex(pattern: @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$");

                if ((string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(LastName)
                    || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPassword)))
                {
                    ShowMessageIsNotValid("يرجى ملئ الحقول الفارغة *", true);
                    return;
                }

                if (!regexEmail.IsMatch(Email.Trim()))
                {
                    ShowMessageIsNotValid("اسم المستخدم غير صحيح *", true);
                    return;
                }

                if (!regexPass.IsMatch(Password))
                {
                    ShowMessageIsNotValid("ادخل احرف كبيرة وصغيرة ورموز واكثر من 8 احرف *", true);
                    return;
                }

                if (Password != ConfirmPassword)
                {
                    ShowMessageIsNotValid("كلمة المرور غير متطابقة", true);
                    return;
                }

                bool isStoredUser = await usersService.IsStoredEmail(Email);
                if (isStoredUser)
                {
                    ShowMessageIsNotValid(" هذا الحساب موجود بالفعل قم بتسجيل الدخول *", true);
                    return;
                }
                
                await usersService.SaveAccountAsync(GetUserInfo());
                var UserInfo = JsonConvert.SerializeObject(GetUserInfo());

                Preferences.Set("UserInfo", UserInfo);
                Preferences.Set("Registered", true);

                App.Current.MainPage = new NavigationPage(new UserFlayoutPage());

                ShowMessageIsNotValid("", false);
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
            CreateAccountCommand?.ChangeCanExecute();
        }
    }
}
