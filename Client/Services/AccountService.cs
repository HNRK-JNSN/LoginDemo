using LoginDemo.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using Blazored.LocalStorage;

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
        private ILocalStorageService _localStorage;

        private HttpClient _httpClient;
        private NavigationManager _navigationManager;

        private string _userKey = "user";

        public User? User { get; private set; }

        public AccountService(
            HttpClient httpClient,
            NavigationManager navigationManager,
            ILocalStorageService localStorage 
        ) {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _localStorage = localStorage;
        }

        public async Task Initialize()
        {
            User = await _localStorage.GetItemAsync<User>(_userKey);
        }

        public async Task Login(Login model)
        {
            var response = await _httpClient.PostAsJsonAsync<Login>("/account/authenticate", model);
            User = await response.Content.ReadFromJsonAsync<User>();            
            await _localStorage.SetItemAsync(_userKey, User);
        }

        public async Task Logout()
        {
            User = null;
            await _localStorage.RemoveItemAsync(_userKey);
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