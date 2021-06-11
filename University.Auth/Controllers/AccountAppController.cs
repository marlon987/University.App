using Microsoft.AspNet.Identity.Owin;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using University.Auth.Models;
using University.BL.DTOs;

namespace University.Auth.Controllers
{
    [RoutePrefix("api/AccountApp")]
    public class AccountAppController : ApiController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountAppController()
        {

        }

        public AccountAppController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? Request.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IHttpActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)

                    return BadRequest();

                // No cuenta los errores de inicio de sesión para el bloqueo de la cuenta
                // Para permitir que los errores de contraseña desencadenen el bloqueo de la cuenta, cambie a shouldLockout: true
                var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:

                        var user = await UserManager.FindByNameAsync(model.Email);

                        var fileName = string.Format("{0}.{1}", user.Id, "Jpg");
                        var filePath = ConfigurationManager.AppSettings["UrlBase"] + @"/Documents/";
                        var userDTO = new UserDTO

                        {
                            Id = user.Id,
                            Email = user.Email,
                            UserName = user.UserName,
                            Image = filePath + fileName

                        };

                        return Ok(new ResponseDTO { Code = 200, Data = userDTO });

                    default:
                        return Ok(new ResponseDTO { Code = 401, Message = "Invalid login attempt." });

                }
            }
            catch (Exception ex)
            {
                return Ok(new ResponseDTO
                {
                    Code = 500,
                    Message = ex.Message
                });
            }
        }

        // POST: /Account/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                        return Ok(new ResponseDTO { Code = 200 });

                    else
                        return Ok(new ResponseDTO { Code = 500, Message = string.Join(", ", result.Errors.Select(x => x)) });

                }

                return Ok(new ResponseDTO { Code = 400, Message = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)) });
            }

            catch (Exception ex)
            {

                return InternalServerError(ex);
            }

        }

        // POST: /Account/Profile
        [HttpPost]
        [Route("Profile")]
        public async Task<IHttpActionResult> Profile(ProfileDTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Nombre del archivo , formato 
                    var fileName = string.Format("{0}.{1}", model.UserID, model.Ext);
                    //Donde se guardara el archivo 
                    var filePath = HttpContext.Current.Server.MapPath("~") + @"\Documents\";
                    var path = filePath + fileName;
                    BL.Helpers.Utils.SaveFile(path, model.Image);

                    return Ok(new ResponseDTO { Code = 200 });
                }

                return Ok(new ResponseDTO { Code = 400, Message = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)) });
            }

            catch (Exception ex)
            {

                return Ok(new ResponseDTO { Code = 500, Message = ex.Message });
            }

        }

        // GET: USER
        [HttpGet]
        [Route("GetUser")]
        public async Task<IHttpActionResult> GetUser(string userID)
        {
            try
            {
                var user = await UserManager.FindByIdAsync(userID);

                if (user != null)
                {

                    var fileName = string.Format("{0}.{1}", user.Id, "Jpg");
                    var filePath = ConfigurationManager.AppSettings["UrlBase"] + @"/Documents/";

                    //Validacion de si el archivo existe en el servidor
                    var path = HttpContext.Current.Server.MapPath("~") + @"\Documents\" + fileName;
                    var isExistFile = BL.Helpers.Utils.IsExistFile(path);

                    var userDTO = new UserDTO

                    {
                        Id = user.Id,
                        Email = user.Email,
                        UserName = user.UserName,
                        Image = isExistFile ? filePath + fileName : string.Empty,
                        ImageBase64 = isExistFile ? Convert.ToBase64String(File.ReadAllBytes(path)) : string.Empty

                    };

                    return Ok(new ResponseDTO { Code = 200, Data = userDTO });
                }
                else
                    return Ok(new ResponseDTO { Code = 404, Message = "Not Found" });
            }

            catch (Exception ex)
            {

                return Ok(new ResponseDTO { Code = 500, Message = ex.Message });
            }

        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(new ResponseDTO { Code = 400, Message = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)) });

                var result = await UserManager.ChangePasswordAsync(model.UserId, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                    return Ok(new ResponseDTO { Code = 200 });

                return Ok(new ResponseDTO { Code = 500, Message = string.Join(", ", result.Errors.Select(x => x)) });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseDTO { Code = 500, Message = ex.Message });
            }
        }

    }
}

