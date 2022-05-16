using LoginDemo.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoginDemo.Client.Services
{
    public interface IAccountService
    {
        User? User { get; }
        Task Initialize();
        Task Login(Login model);
        Task Logout();
        Task CreateUser(AddUser model);

    }

    public class AccountService : IAccountService
    {
        private HttpClient _httpClient;
        private NavigationManager _navigationManager;
        private ILocalStorageService _localStorageService;
        private string _userKey = "user";

        public User? User { get; private set; }

        public AccountService(
            HttpClient httpClient,
            NavigationManager navigationManager,
            ILocalStorageService localStorageService
        ) {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
        }

        public async Task Initialize()
        {
            User = await _localStorageService.GetItem<User>(_userKey);
        }

        public async Task Login(Login model)
        {
            var response = await _httpClient.PostAsJsonAsync<Login>("/account/authenticate", model);
            User = await response.Content.ReadFromJsonAsync<User>();            
            await _localStorageService.SetItem(_userKey, User);
        }

        public async Task Logout()
        {
            User = null;
            await _localStorageService.RemoveItem(_userKey);
            _navigationManager.NavigateTo("/login");
        }

        public async Task CreateUser(AddUser model)
        {
            var response = await _httpClient.PostAsJsonAsync<AddUser>("/account/user", model);
            // TODO: error handling
            _navigationManager.NavigateTo("/");
        }

    }
}