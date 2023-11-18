using NativeMedia;
using Newtonsoft.Json;
using System.Threading.Tasks;
using TT_App.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TT_App.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
     //   public Command ChangeProfileImageCommand { get; }

        public Command GetUserInfoCommand { get; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public int UserID { get; set; }

        public ProfileViewModel()
        {
           // ChangeProfileImageCommand = new Command(async () => await ChangeProfileImageAsync(), () => !IsBusy);
            GetUserInfoCommand = new Command(() => FillUserInfo());
            GetUserInfoCommand.Execute(null);
        }

        private string myLabel;
        public string MyLabel
        {
            get
            {
                return myLabel;
            }
            set
            {
                myLabel = value;
                OnPropertyChanged();
            }
        }
        public async Task ChangeProfileImageAsync()
        {
            try
            {
                IsBusy = true;
                var result = await MediaGallery.PickAsync(1, MediaFileType.Image, MediaFileType.Video);

                if (result?.Files == null)
                    return;
                foreach (var item in result.Files)
                {
                    var filename = item.NameWithoutExtension;
                    var extinsion = item.Extension;

                    MyLabel = extinsion;
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
        private void FillUserInfo()
        {
            User user = JsonConvert.DeserializeObject<User>(Preferences.Get("UserInfo", ""));
            UserName = user.firstName + " " + user.lastName;
            Email = user.email;
            UserID = user.userID;

            OnPropertyChanged(nameof(UserName));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(UserID));

        }

        public override void CommandStateChanged()
        {
           // ChangeProfileImageCommand?.ChangeCanExecute();

        }
    }
}
