using System;
using System.Collections.Generic;
using System.Text;

namespace University.App.Helpers
{
    public class Endpoints
    {
        public static string URL_BASE_UNIVERSITY_AUTH { get; set; }
        = "https://university-auth.azurewebsites.net/";

        public static string LOGIN { get; set; } = "api/AccountApp/Login/";
        public static string REGISTER { get; set; } = "api/AccountApp/Register/";
        public static string PROFILE { get; set; } = "api/AccountApp/Profile/";
        public static string GET_USER { get; set; } = "api/AccountApp/GetUser/";

        public static string CHANGE_PASSWORD { get; set; } = "/api/AccountApp/ChangePassword/";

    }
}
