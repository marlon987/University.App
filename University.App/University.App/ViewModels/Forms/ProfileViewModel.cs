using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using University.App.Helpers;
using University.App.Views.Forms;
using University.BL.DTOs;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class ProfileViewModel : BaseViewModel
    {
        #region Attributes
        private MediaFile _file;
        private UserDTO _user;
        private ApiService _apiService;
        private ImageSource _imageSource;
        #endregion

        #region Properties
        public UserDTO User
        {
            get { return this._user; }
            set { this.SetValue(ref this._user, value); }
        }

        public ImageSource ImageSource
        {
            get { return this._imageSource; }
            set { this.SetValue(ref this._imageSource, value); }
        }
        #endregion

        #region Commands
        public Command AddImageCommand { get; set; }
        public Command EditProfileCommand { get; set; }
        public Command GetUserCommand { get; set; }
        public Command ChangePasswordCommand { get; set; }
        #endregion

        #region Constructor

        public ProfileViewModel()
        {
            this._apiService = new ApiService();

            this.AddImageCommand = new Command(AddImage);
            this.EditProfileCommand = new Command(EditProfile);
            this.GetUserCommand = new Command(GetUser);
            this.GetUserCommand.Execute(null);
            this.ChangePasswordCommand = new Command(ChangePassword);
        }
        #endregion

        #region Methods

        async void GetUser()
        {
            try
            {
                //Obtencion de la informacion del usuario
                var userID = Helpers.Settings.UserID;
                var responseDTO = await _apiService.RequestAPI<UserDTO>(Helpers.Endpoints.URL_BASE_UNIVERSITY_AUTH,
                    Helpers.Endpoints.GET_USER + "?userID=" + userID,
                    null,
                    ApiService.Method.Get,
                    true);

                if (responseDTO.Code == 200)
                {
                    //Informacion del usuario
                    this.User = (UserDTO)responseDTO.Data;
                    this.ImageSource = "profile";
                    // Se valida si la imagen es diferente de vacia
                    if (!string.IsNullOrEmpty(this.User.Image))
                    {
                        //Si es valida se lee la informacion en un stream
                        this.ImageSource = ImageSource.FromStream(() =>
                        {
                            //Convierta de base64 a bytes y se hace el memorystream para leerlo y asignarlo al imagesourse
                            var byteArray = Convert.FromBase64String(this.User.ImageBase64);
                            return new MemoryStream(byteArray);
                        });
                    }


                }
                else
                    await Application.Current.MainPage.DisplayAlert(Languages.Notification, responseDTO.Message, Languages.Accept);
            }
            catch (Exception ex)
            {

                await Application.Current.MainPage.DisplayAlert(Languages.Notification, ex.Message, Languages.Accept);
            }
        }

        async void EditProfile()
        {
            try
            {
                var imageBase64 = string.Empty;
                if (this._file != null)
                {
                    var imageArray = Helpers.FileHelper.ReadFully(this._file.GetStream());
                    imageBase64 = Convert.ToBase64String(imageArray);
                }

                var userID = Helpers.Settings.UserID;
                var profileDTO = new ProfileDTO
                {
                    UserID = userID,
                    Image = imageBase64,
                    Ext = "jpg"
                };

                var responseDTO = await _apiService.RequestAPI<ResponseDTO>(Helpers.Endpoints.URL_BASE_UNIVERSITY_AUTH,
                    Helpers.Endpoints.PROFILE,
                    profileDTO,
                    ApiService.Method.Post,
                    true);

                if (responseDTO.Code == 200)
                    MainViewModel.GetInstance().Profile = new ProfileViewModel();

                else
                    await Application.Current.MainPage.DisplayAlert(Languages.Notification, responseDTO.Message, Languages.Accept);
            }
            catch (Exception ex)
            {

                await Application.Current.MainPage.DisplayAlert(Languages.Notification, ex.Message, Languages.Accept);
            }
        }

        async void AddImage()
        {
            try
            {
                await CrossMedia.Current.Initialize();

                var source = await Application.Current.MainPage.DisplayActionSheet(Languages.Whereareyougoingtotakethephoto,
                    Languages.Accept,
                    null,
                    Languages.Takeanewpicture,
                    Languages.Fromthegallery);

                if (source.Equals(Languages.Accept))
                {
                    this._file = null;
                    return;

                }
                else if (source.Equals(Languages.Takeanewpicture))
                {
                    this._file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "Sample.jpg",
                        PhotoSize = PhotoSize.Small
                    });
                }

                else
                    //Tomar foto de la galeria
                    this._file = await CrossMedia.Current.PickPhotoAsync();

                if (this._file != null)
                {
                    this.ImageSource = ImageSource.FromStream(() =>
                    {
                        var stream = _file.GetStream();
                        return stream;

                    });
                }
            }

            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Notification, ex.Message, Languages.Accept);
            }
        }

        async void ChangePassword()
        {
            MainViewModel.GetInstance().ChangePassword = new ChangePasswordViewModel();
             await App.Navigator.PushAsync(new ChangePasswordPage());
        }
        #endregion
    }
}
