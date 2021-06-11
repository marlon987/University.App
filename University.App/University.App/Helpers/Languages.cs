using University.App.Interfaces;
using University.App.Resources;
using Xamarin.Forms;

namespace University.App.Helpers
{
    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Accept { get { return Resource.Accept; } }
        public static string EditProfile { get { return Resource.EditProfile; } }
        public static string Notification { get { return Resource.Notification; } }
        public static string NoInternetConnection { get { return Resource.NoInternetConnection; } }
        public static string FieldsRequired { get { return Resource.FieldsRequired; } }
        public static string Send { get { return Resource.Send; } }
        public static string ChangePasswordSuccessfull { get { return Resource.ChangePasswordSuccessfull; } }
        public static string Profile { get { return Resource.Profile; } }
        public static string About { get { return Resource.About; } }
        public static string Logout { get { return Resource.Logout; } }
        public static string Whereareyougoingtotakethephoto { get { return Resource.Whereareyougoingtotakethephoto; } }
        public static string Takeanewpicture { get { return Resource.Takeanewpicture; } }
        public static string Fromthegallery { get { return Resource.Fromthegallery; } }



    }
}
