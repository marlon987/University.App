using System.Collections.ObjectModel;
using University.App.Helpers;
using University.App.ViewModels.Forms;
using University.App.ViewModels.Menu;

namespace University.App.ViewModels
{
    public class MainViewModel
    {        
        public LoginViewModel Login { get; set; } 
        public ProfileViewModel Profile { get; set; }
        public RegisterViewModel Register { get; set; }      
        public ObservableCollection<MenuItemViewModel> Menu { get; set; }
        public AboutViewModel About { get; set; }
        public PQRSViewModel PQRS { get; set; }
        public ChangePasswordViewModel ChangePassword { get; set; }

        #region Constructors
        public MainViewModel()
        {
            instance = this;
            this.LoadMenu();

            this.Login = new LoginViewModel();

            this.Register = new RegisterViewModel();
        }
        #endregion

        #region Methods
        private void LoadMenu()
        {
            this.Menu = new ObservableCollection<MenuItemViewModel>
            {                
                new MenuItemViewModel
                {
                    Icon = "ic_person_pin",
                    PageName = "ProfilePage",
                    Title = Languages.Profile
                },
                new MenuItemViewModel
                {
                    Icon = "ic_help",
                    PageName = "PQRSPage",
                    Title = "PQRS"
                },
                 new MenuItemViewModel
                {
                    Icon = "ic_info",
                    PageName = "AboutPage",
                    Title = Languages.About
                },
                 new MenuItemViewModel
                {
                    Icon = "ic_exit_to_app",
                    PageName = "LoginPage",
                    Title = Languages.Logout
                }
            };
        }
        #endregion

        #region Singleton
        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
                return new MainViewModel();

            return instance;
        }
        #endregion
    }
}
