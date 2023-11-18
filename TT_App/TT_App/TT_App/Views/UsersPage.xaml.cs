using TT_App.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TT_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsersPage : ContentPage
    {
        public UsersPage()
        {
            InitializeComponent();

        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var user = e.Item as User;
            DisplayAlert("Tapped" ,$"{user.firstName} {user.lastName}\n{user.email}","OK");
        }
    }
}