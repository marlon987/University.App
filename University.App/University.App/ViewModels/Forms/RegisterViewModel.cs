using System;
using System.Collections.Generic;
using System.Text;
using University.App.Views.Forms;
using University.App.Views.Menu;
using University.BL.DTOs;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class RegisterViewModel : BaseViewModel
    {
        #region Attributes
        private string _email;
        private string _password;
        private string _confirmpassword;
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

        public string ConfirmPassword
        {
            get { return this._confirmpassword; }
            set { this.SetValue(ref this._confirmpassword, value); }
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
        public Command RegisterCommand { get; set; }
        public Command BackCommand { get; set; }
        #endregion

        #region Methods
        async void Register()
        {
            try
            {
                this.IsRunning = true;
                this.IsEnabled = false;

                if (!await _apiService.CheckConnection())
                {
                    this.IsRunning = false;
                    this.IsEnabled = true;

                    await Application.Current.MainPage.DisplayAlert("Notification", "No Internet Connection", "Accept");
                    return;
                }

                if (string.IsNullOrEmpty(this.Email) || string.IsNullOrEmpty(this.Password) || string.IsNullOrEmpty(this.ConfirmPassword))
                {
                    this.IsRunning = false;
                    this.IsEnabled = true;

                    await Application.Current.MainPage.DisplayAlert("Notification", "The Fields Are Required", "Accept");
                    return;
                }

                var registerDTO = new RegisterDTO
                {
                    Email = this.Email,
                    Password = this.Password,
                    ConfirmPassword = this.ConfirmPassword
                };

                var responseDTO = await _apiService.RequestAPI<UserDTO>(Helpers.Endpoints.URL_BASE_UNIVERSITY_AUTH,
                    Helpers.Endpoints.REGISTER,
                    registerDTO,
                    ApiService.Method.Post, true);

                if (responseDTO.Code == 200)
                {
                    await Application.Current.MainPage.DisplayAlert("Notification", "Thanks For Register", "Accept");

                    Application.Current.MainPage = new LoginPage();

                }

                else
                    await Application.Current.MainPage.DisplayAlert("Notification", responseDTO.Message, "Accept");

                this.IsRunning = false;
                this.IsEnabled = true;


            }
            catch (Exception ex)
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert("Notification", ex.Message, "Accept");
            }
        }

        async void Back()
        {

            Application.Current.MainPage = new LoginPage();

        }
        #endregion



        #region Constructors
        public RegisterViewModel()
        {
            this.IsEmailValid = this.IsEnabled = true;
            this.IsRunning = false;

            this._apiService = new ApiService();

            this.RegisterCommand = new Command(Register);

            this.BackCommand = new Command(Back);
        }

        #endregion
    }
}
