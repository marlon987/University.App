using University.App.ViewModels.Forms;
using University.App.Views.Forms;
using Xamarin.Forms;

namespace University.App.ViewModels.Menu
{
    public class MenuItemViewModel
    {
        #region Properties
        public string Icon { get; set; }
        public string Title { get; set; }
        public string PageName { get; set; }
        #endregion

        #region Constructors
        public MenuItemViewModel()
        {
            this.GoToCommand = new Command(GoTo);
        }
        #endregion

        #region Commands
        public Command GoToCommand { get; set; }
        #endregion

        #region Methods
        async void GoTo()
        {
            await App.Navigator.PopToRootAsync();

            if (this.PageName.Equals("LoginPage"))
            {
                MainViewModel.GetInstance().Login = new LoginViewModel();
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
            else if (this.PageName.Equals("ProfilePage"))
            {
				MainViewModel.GetInstance().Profile = new ProfileViewModel();
                await App.Navigator.PushAsync(new ProfilePage());      
            }
            else if (this.PageName.Equals("AboutPage"))
            {
                MainViewModel.GetInstance().About = new AboutViewModel();
                await App.Navigator.PushAsync(new AboutPage());
            }
            else if (this.PageName.Equals("PQRSPage"))
            {
                MainViewModel.GetInstance().PQRS = new PQRSViewModel();
                await App.Navigator.PushAsync(new PQRSPage());
            }
        }
        #endregion
    }
}
