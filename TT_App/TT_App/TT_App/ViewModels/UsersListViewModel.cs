using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TT_App.Models;
using TT_App.Services;
using Xamarin.Forms;

namespace TT_App.ViewModels
{
    public class UsersListViewModel : BaseViewModel
    {
        public List<User> Users { get; set; }

        public Command GetUsersCommand { get; }

        public UsersListViewModel()
        {
            usersService = new UsersService();

            GetUsersCommand = new Command(async () => await GetListOfUsersAsync());
            GetUsersCommand.Execute(null);
        }
        public async Task GetListOfUsersAsync()
        {
            Users = await usersService.GetAllAccountsAsync();
            OnPropertyChanged(nameof(Users));
        }

        public override void CommandStateChanged()
        {
            throw new NotImplementedException();
        }
    }
}
