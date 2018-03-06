using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SlotCarsGo_Server.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Configuration;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.PeopleService.v1;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2.Flows;
using System.Reflection;
using Google.Apis.Auth.OAuth2.Web;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2.Mvc;

namespace SlotCarsGo_Server.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
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
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;

                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email, Username = loginInfo.ExternalIdentity.Name });
            }
        }


        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }

                // Add user to DB
                var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
                //var result = await UserManager.CreateAsync(user);
                var result = IdentityResult.Success;
                if (result.Succeeded)
                {
                    try
                    {
                        // Retrieve new users profile image from chosen social media
                        string imageUrl = String.Empty;
                        if (info.Login.LoginProvider == "Facebook")
                        {
                            var fbClient = new Facebook.FacebookClient();
                            dynamic fbToken = fbClient.Get("oauth/access_token", new
                            {
                                client_id = ConfigurationManager.AppSettings["FacebookAppId"].ToString(),
                                client_secret = ConfigurationManager.AppSettings["FacebookAppSecret"].ToString(),
                                grant_type = "client_credentials"
                            });
                            fbClient = new Facebook.FacebookClient(fbToken["access_token"]);
                            dynamic fbImageResult = fbClient.Get($"{info.Login.ProviderKey}?fields=picture.width(200).height(200)");
                            imageUrl = fbImageResult["picture"]["data"]["url"];
                        }
                        else if (info.Login.LoginProvider == "Google")
                        {
                            string userId = info.Login.ProviderKey;
                            this.Session["user"] = info.Login.ProviderKey;
                            var resultToken = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).
                AuthorizeAsync(CancellationToken.None);
                            UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                                new ClientSecrets
                                {
                                    ClientId = ConfigurationManager.AppSettings["GoogleClientId"].ToString(),
                                    ClientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"].ToString()
                                },
                                new[] { "coverPhotos"},
                                 userId,
                                CancellationToken.None,
                                new FileDataStore(@"C:\users\tango\GoogleAuth", true)).Result;

                            // Create the service.
                            PeopleServiceService peopleService = new PeopleServiceService(new BaseClientService.Initializer()
                            {
                                HttpClientInitializer = credential,
                                ApplicationName = "APP_NAME",
                            });
                            PeopleResource.GetRequest peopleRequest = peopleService.People.Get("me");
                            peopleRequest.PersonFields = "coverPhotos";
                            var profile = peopleRequest.Execute();
                            // Save users profile image and update user model in DB.
                            // Create OAuth credential.
                            /*
                                                                                string credPath = ConfigurationManager.AppSettings["BuildType"].ToString().Equals("DEBUG")
                                                                                    ? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                                                                                    : Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                                                                                credPath = Path.Combine("/Credentials/", Assembly.GetExecutingAssembly().GetName().Name);
                                                                                credPath = "~/AppData/Credentials/SlotCarsGo";
                            */

                            /*
                                                                                UserCredential credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                                                                                    new ClientSecrets
                                                                                    {
                                                                                        ClientId = ConfigurationManager.AppSettings["GoogleClientId"].ToString(),
                                                                                        ClientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"].ToString()
                                                                                    },
                                                                                    new[] { "profile", "https://www.googleapis.com/auth/contacts.readonly" },
                                                                                    "me",
                                                                                    CancellationToken.None
                            //                                                        ,new FileDataStore(credPath, true)
                                                                                );


                                                                                ClientSecrets secrets = new ClientSecrets()
                                                                                {
                                                                                    ClientId = ConfigurationManager.AppSettings["GoogleClientId"].ToString(),
                                                                                    ClientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"].ToString()
                                                                                };

                                                                                var token = new TokenResponse { RefreshToken = "3600" };
                                                                                var credential = new UserCredential(new GoogleAuthorizationCodeFlow(
                                                                                    new GoogleAuthorizationCodeFlow.Initializer
                                                                                    {
                                                                                        ClientSecrets = secrets
                                                                                    }),
                                                                                    "me",
                                                                                    token);

                                                                                // Create the service.
                                                                                var service = new PeopleService(new BaseClientService.Initializer()
                                                                                {
                                                                                    HttpClientInitializer = credential,
                                                                                    ApplicationName = "SlotCarsGo",
                                                                                });
                            */
                            // Create OAuth credential.
                            /*
                                                            UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                                                                new ClientSecrets
                                                                {
                                                                    ClientId = "CLIENT_ID",
                                                                    ClientSecret = "CLIENT_SECRET"
                                                                },
                                                                new[] { "profile",
                                "https://www.googleapis.com/auth/contacts.readonly" },
                                                                 "me",
                                                                CancellationToken.None).Result;

                                                            // Create the service.
                                                            PeopleService peopleService = new PeopleService(new BaseClientService.Initializer()
                                                            {
                                                                HttpClientInitializer = credential,
                                                                ApplicationName = "APP_NAME",
                                                            });

                                                            Google.Apis.PeopleService.v1.PeopleResource.ConnectionsResource.ListRequest peopleRequest =
                                                                peopleService.People.Connections.List("people/me");
                                                            peopleRequest.PersonFields = "names,emailAddresses";
                                                            ListConnectionsResponse connectionsResponse = peopleRequest.Execute();
                            */
                            /*
                            this.Session["user"] = info.Login.ProviderKey;
                                                                var token = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).AuthorizeAsync(CancellationToken.None);

                                                                if (token.Credential != null)
                                                                {
                                                                    var service = new PeopleService(new BaseClientService.Initializer
                                                                    {
                                                                        HttpClientInitializer = token.Credential,
                                                                        ApplicationName = "SlotCarsGo"
                                                                    });

                                                                    // YOUR CODE SHOULD BE HERE..
                                                                    // SAMPLE CODE:

                                                                    //                                    PeopleResource.GetRequest peopleRequest = service.People.Get("people/me");
                                                                    //                                                    peopleRequest.RequestMaskIncludeField = "person.names,person.emailAddresses,person.coverPhotos";
                                                                    Person profile = service.People.Get("people/me").Execute();
                                                                    imageUrl = profile.CoverPhotos.ToString();
                                
                            }
                                                                */
                            /*
                                                            var token = ConfigurationManager.AppSettings["GoogleClientId"];
                                                            string UserId = info.Login.ProviderKey;
                                                            string uri = $@"https://people.googleapis.com/v1/people/{UserId}?personFields=coverPhotos&fields=coverPhotos&key={token}";

                                                            string googleImageResult = await httpClient.GetStringAsync(uri);
                                                            dynamic json = JsonConvert.DeserializeObject(googleImageResult);
                                                            imageUrl = json["url"];

                                                            /*
                                                            // Call auth service to get a token
                                                            // string token = call
                                                            // embed token in http call spomehow, API object?

                                                            GoogleRedirectAuthorizationBroker.RedirectUri = returnUrl;
                                                            UserCredential credential = await GoogleRedirectAuthorizationBroker.AuthorizeAsync(
                                                                new ClientSecrets
                                                                {
                                                                    ClientId = ConfigurationManager.AppSettings["GoogleClientId"].ToString(),
                                                                    ClientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"].ToString()
                                                                },
                                                                new[] { PeopleService.Scope.UserinfoProfile },
                                                                UserId,
                                                                CancellationToken.None);

                                                            // Create the service.
                                                            var service = new PeopleService(new BaseClientService.Initializer()
                                                            {
                                                                HttpClientInitializer = credential,
                                                                ApplicationName = "GetCoverPhotoService",
                                                            });
                                                                                            var userProfile = await service.HttpClient.GetStringAsync(uri2);

                                                        /////
                                                                                            People.Mylibrary.Bookshelves.List().ExecuteAsync();

                                                                                            GoogleAuthorizationCodeFlow flow;
                                                                                            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                                                                                                new ClientSecrets
                                                                                                {
                                                                                                    ClientId = "PUT_CLIENT_ID_HERE",
                                                                                                    ClientSecret = "PUT_CLIENT_SECRETS_HERE"
                                                                                                },
                                                                                                new[] { PeopleService.Scope.UserinfoProfile },
                                                                                                UserId,
                                                                                                CancellationToken.None);
                                                                                            var uri = Request.Url.ToString();
                                                                                            var code = Request["code"];
                                                                                            if (code != null)
                                                                                            {
                                                                                                var token = flow.ExchangeCodeForTokenAsync(UserId, code,
                                                                                                    uri.Substring(0, uri.IndexOf("?")), CancellationToken.None).Result;

                                                                                                // Extract the right state.
                                                                                                var oauthState = AuthWebUtility.ExtracRedirectFromState(
                                                                                                    flow.DataStore, UserId, Request["state"]).Result;
                                                                                                Response.Redirect(oauthState);
                                                            */
                        }

                        // Save users profile image
                        using (WebClient webClient = new WebClient())
                        {
                            webClient.DownloadFile(imageUrl, $"{Server.MapPath(Url.Content("~/Content/Images/Users"))}/{user.Id}.jpg");
                        }

                        user.ImageName = $"{user.Id}.jpg";
                    }
                    catch (Exception)
                    {
                        user.ImageName = "0.jpg";
                    }

                    // Update user model in DB.
                    UserManager.Update(user);
                    
                    // Login
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: true, rememberBrowser: true);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}