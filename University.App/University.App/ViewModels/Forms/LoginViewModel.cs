using System;
using System.Globalization;
using System.Text.RegularExpressions;
using University.App.Helpers;
using University.App.Views.Forms;
using University.App.Views.Menu;
using University.BL.DTOs;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class LoginViewModel : BaseViewModel
    {
        #region Attributes
        private string _email;
        private string _password;
        private bool _isEmailValid;
        private bool _isEnabled;
        private bool _isRunning;
        private ApiService _apiService;
        #endregion

        #region Properties
        public string Email
        {
            get { return this._email; }
            set { this.SetValue(ref this._email, value); }
        }

        public string Password
        {
            get { return this._password; }
            set { this.SetValue(ref this._password, value); }
        }

        public bool IsEmailValid
        {
            get { return this._isEmailValid; }
            set { this.SetValue(ref this._isEmailValid, value); }
        }

        public bool IsEnabled
        {
            get { return this._isEnabled; }
            set { this.SetValue(ref this._isEnabled, value); }
        }

        public bool IsRunning
        {
            get { return this._isRunning; }
            set { this.SetValue(ref this._isRunning, value); }
        }
        #endregion

        #region Commands
        public Command LoginCommand { get; set; }
        public Command RegisterCommand { get; set; }

        #endregion

        #region Methods

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }


        async void Login()
        {
            try
            {

                this.IsEmailValid = IsValidEmail(this.Email);

                if (!this.IsEmailValid)
                    return;

                this.IsRunning = true;
                this.IsEnabled = false;

                if (!await _apiService.CheckConnection())
                {
                    this.IsRunning = false;
                    this.IsEnabled = true;

                    await Application.Current.MainPage.DisplayAlert(Languages.Notification,Languages.NoInternetConnection, Languages.Accept);
                    return;
                }

                if (string.IsNullOrEmpty(this.Email) || string.IsNullOrEmpty(this.Password))
                {
                    this.IsRunning = false;
                    this.IsEnabled = true;

                    await Application.Current.MainPage.DisplayAlert(Languages.Notification, Languages.FieldsRequired, Languages.Accept);
                    return;
                }

                var loginDTO = new LoginDTO
                {
                    Email = this.Email,
                    Password = this.Password
                };

                var responseDTO = await _apiService.RequestAPI<UserDTO>(Helpers.Endpoints.URL_BASE_UNIVERSITY_AUTH,
                    Helpers.Endpoints.LOGIN,
                    loginDTO,
                    ApiService.Method.Post, true);

                if (responseDTO.Code == 200)
                {
                    var user = (UserDTO)responseDTO.Data;
                    Helpers.Settings.UserID = user.Id;

                    Application.Current.MainPage = new MasterPage();

                }

                else
                    await Application.Current.MainPage.DisplayAlert(Languages.Notification, responseDTO.Message, Languages.Accept);

                this.IsRunning = false;
                this.IsEnabled = true;

            }
            catch (Exception ex)
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(Languages.Notification, ex.Message, Languages.Accept);
            }

        }
        async void Register()
        {
            Application.Current.MainPage = new RegisterPage();
        }
        #endregion  

        #region Constructors
        public LoginViewModel()
        {
            this.IsEmailValid = this.IsEnabled = true;
            this.IsRunning = false;

            this._apiService = new ApiService();
            this.LoginCommand = new Command(Login);
            this.RegisterCommand = new Command(Register);
        }

        #endregion
    }
}
