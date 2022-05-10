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
        Task Register(AddUser model);
        //Task<IList<User>> GetAll();
        //Task<User> GetById(string id);
        //Task Delete(string id);
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
            User = await _httpClient.PostAsJsonAsync<Login>("/account/authenticate", model).Result.Content.ReadFromJsonAsync<User>();

            //User = await response.Content.ReadFromJsonAsync<User>();
            
            Console.WriteLine("Hello");

            await _localStorageService.SetItem(_userKey, User);
        }

        public async Task Logout()
        {
            User = null;
            await _localStorageService.RemoveItem(_userKey);
            _navigationManager.NavigateTo("/login");
        }

        public async Task Register(AddUser model)
        {
            // TODO: get users from controller by id;
            //return null;
        }

        // public async Task<IList<User>> GetAll()
        // {
        //     //     // TODO: get users from controller by id;
        // }

        // public async Task<User> GetById(string id)
        // {
        //     // TODO: get users from controller by id;
        // }

    }
}