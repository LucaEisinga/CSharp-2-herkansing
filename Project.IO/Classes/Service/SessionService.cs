using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace Project.IO.Classes.Service
{
    internal class SessionService
    {

        private ILocalStorageService? _localStorageService;

        public async Task IsLoggedInAsync(bool isLoggedIn)
        {
            await _localStorageService.SetItemAsync("IsLoggedIn", isLoggedIn);
        }

        public async Task<string> GetIsLoggedInAsync()
        {
            return await _localStorageService.GetItemAsync<string>("IsLoggedIn");
        }

        public async Task SetUserIdAsync(string userId)
        {
            await _localStorageService.SetItemAsync("UserId", userId);
        }

        public async Task<string> GetUserIdAsync()
        {
            return await _localStorageService.GetItemAsync<string>("UserId");
        }


    }
}
