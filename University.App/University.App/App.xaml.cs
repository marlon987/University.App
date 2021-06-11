using System;
using University.App.Views.Forms;
using University.App.Views.Menu;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace University.App
{
    public partial class App : Application
    {
        public static NavigationPage Navigator   { get; internal set; }

        public App()
        {
            InitializeComponent();

            MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
