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
using SurveysManagement.Web.Models;
using SurveysManagement.DataAccess.Entities;
using System.Collections.Generic;
using System.Net;

namespace SurveysManagement.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private SurveyModel db = new SurveyModel();

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
        [Authorize(Roles = "Admin")]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result =await UserManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {                    
                    //SurveyModel db = new SurveyModel();
                    AspNetUser newUser = db.AspNetUsers.FirstOrDefault(x => x.Email == user.Email);
                    if (model.Role == "1")
                    {
                        UserManager.AddToRole(newUser.Id, "User");
                    }
                    else if (model.Role == "2")
                    {
                        UserManager.AddToRole(newUser.Id, "Admin");
                    }
                    else if (model.Role == "3")
                    {
                        UserManager.AddToRole(newUser.Id, "Client");
                    }
                    db.Users.Add(new User
                    {
                        ASPUserId = newUser.Id,
                        Name = model.Name,
                        Major = model.Major,
                        Phone = model.Phone,
                        ShiftDescription = model.ShiftDescription,
                        Gender = model.Gender,
                        Area = model.Area,
                        BirthDate = model.BirthDate,
                        Education = model.Education,
                        CreationDate = DateTime.Now,
                        City = model.City,
                        ExpiryDate = model.ExpiryDate,
                        ShiftEnd = model.ShiftEnd,
                        ShiftStart = model.ShiftStart,
                        Street = model.Street

                    });
                    db.SaveChanges();
                    //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
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
        // GET: /Account/Register
        [AllowAnonymous]
        [Authorize(Roles = "Admin")]
        public ActionResult ViewUserInfo(int id)
        {
            ViewUserInfoViewModel model = new ViewUserInfoViewModel();
            SurveyModel models = new SurveyModel();
            User _User = models.Users.FirstOrDefault(x => x.Id == id);
            model.Id = _User.Id;
            model.RoleId = models.AspNetUsers != null && models.AspNetUsers.Count() > 0 && models.AspNetUsers.FirstOrDefault(x => x.Id == _User.ASPUserId) != null && models.AspNetUsers.FirstOrDefault(x => x.Id == _User.ASPUserId).AspNetRoles != null ? models.AspNetUsers.FirstOrDefault(x => x.Id == _User.ASPUserId).AspNetRoles.FirstOrDefault().Id : "0";
            model.Name = _User.Name;
            model.Major = _User.Major;
            model.Phone = _User.Phone;
            model.ShiftDescription = _User.ShiftDescription;
            model.Gender = _User.Gender;
            model.Area = _User.Area;
            //model.BirthDate = _User.BirthDate;
            model.Education = _User.Education;
            //modelationDate = model.CreationDate;
            model.City = _User.City;
            model.ExpiryDate = _User.ExpiryDate;
            model.ShiftEnd = _User.ShiftEnd;
            model.ShiftStart = _User.ShiftStart;
            model.Street = _User.Street;
            return View(model);
        }
        
        //
        // GET: /Account/Register
        [AllowAnonymous]
        [Authorize(Roles = "Admin")]
        public ActionResult EditUser(int id)
        {            
            EditUserViewModel model = new EditUserViewModel();
            SurveyModel models = new SurveyModel();
            User _User = models.Users.FirstOrDefault(x => x.Id == id);
            model.Id = _User.Id;
            model.RoleId = models.AspNetUsers != null && models.AspNetUsers.Count() > 0 && models.AspNetUsers.FirstOrDefault(x => x.Id == _User.ASPUserId) != null && models.AspNetUsers.FirstOrDefault(x => x.Id == _User.ASPUserId).AspNetRoles != null ? models.AspNetUsers.FirstOrDefault(x => x.Id == _User.ASPUserId).AspNetRoles.FirstOrDefault().Id : "0";
            model.Name = _User.Name;
            model.Major = _User.Major;
            model.Phone = _User.Phone;
            model.ShiftDescription = _User.ShiftDescription;
            model.Gender = _User.Gender;
            model.Area = _User.Area;
            //model.BirthDate = _User.BirthDate;
            model.Education = _User.Education;
            //modelationDate = model.CreationDate;
            model.City = _User.City;
            model.ExpiryDate = _User.ExpiryDate;
            model.ShiftEnd = _User.ShiftEnd;
            model.ShiftStart = _User.ShiftStart;
            model.Street = _User.Street;
            return View(model);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = db.Users.FirstOrDefault(x => x.Id == model.Id);
                
                user.Name = model.Name;
                user.Major = model.Major;
                user.Phone = model.Phone;
                user.ShiftDescription = model.ShiftDescription;
                user.Gender = model.Gender;
                user.Area = model.Area;
                user.BirthDate = model.BirthDate;
                user.Education = model.Education;
                //CreationDate = model.CreationDate;
                user.City = model.City;
                user.ExpiryDate = model.ExpiryDate;
                user.ShiftEnd = model.ShiftEnd;
                user.ShiftStart = model.ShiftStart;
                user.Street = model.Street;
                db.SaveChanges();

                AspNetUser newUser = db.AspNetUsers.FirstOrDefault(x => x.Id == user.ASPUserId);
                if (model.RoleId == "1")
                {
                    UserManager.RemoveFromRoles(newUser.Id, new string[] {"User","Admin","Client"});
                    UserManager.AddToRole(newUser.Id, "User");
                }
                else if (model.RoleId == "2")
                {
                    UserManager.RemoveFromRoles(newUser.Id, new string[] { "User", "Admin", "Client" });
                    UserManager.AddToRole(newUser.Id, "Admin");
                }
                else if (model.RoleId == "3")
                {
                    UserManager.RemoveFromRoles(newUser.Id, new string[] { "User", "Admin", "Client" });
                    UserManager.AddToRole(newUser.Id, "Client");
                }

                return RedirectToAction("Index", "Home");
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
        // GET: /Account/AdminResetPassword
        [Authorize(Roles = "Admin")]
        public ActionResult AdminResetPassword(int id)
        {
            User user = db.Users.FirstOrDefault(x => x.Id == id);            
            return user == null ? View("Error") : View(new ResetPasswordViewModel {Email = UserManager.FindById(user.ASPUserId).Email, Code = UserManager.GeneratePasswordResetToken(UserManager.FindById(user.ASPUserId).Id) });
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AdminResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("Index", "Home");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("UsersList", "Account");
            }
            AddErrors(result);
            return View();
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
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
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
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
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

        
        [Authorize(Roles = "Admin")]
        public ActionResult UsersList()
        {
            //var surveys = db.Surveys.Include(s => s.Category);
            return View();
        }

        [AllowAnonymous]
        public ActionResult GetUserRole(string username)
        {
            JsonResult result = new JsonResult();
            var role = db.AspNetUsers.Include("AspNetRoles").FirstOrDefault(x => x.UserName == username).AspNetRoles.FirstOrDefault();
            result = this.Json(new {Text = role.Name});
            return result;
        }
        
        
        [Authorize(Roles = "Admin")]
        public ActionResult getUsers()
        {
            JsonResult result = new JsonResult();

            try
            {
                // Initialization.
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                // Loading.
                //SurveyModel db = new SurveyModel();
                IQueryable<RegisterViewModel> data = from usr in db.Users
                join reg in db.AspNetUsers.Include("AspNetRoles") on usr.ASPUserId equals reg.Id where(usr.isDeleted != true)
                select new RegisterViewModel {
                    Id = usr.Id,
                    Name = usr.Name,
                    Email = reg.Email,
                    Phone = usr.Phone,
                    Role = reg.AspNetRoles.Count > 0 ? reg.AspNetRoles.FirstOrDefault().Name : "-"
                };

                // Total record count.
                int totalRecords = data.Count();

                // Verification.
                List<RegisterViewModel> resultList;

                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    data = data.Where(p => p.Name.ToString().ToLower().Contains(search.ToLower())
                    || p.Email.ToString().ToLower().Contains(search.ToLower())
                    || p.Phone.ToString().ToLower().Contains(search.ToLower())
                    || p.Education.ToString().ToLower().Contains(search.ToLower())
                    || p.Area.ToString().ToLower().Contains(search.ToLower())
                    || p.BirthDate.ToString().ToLower().Contains(search.ToLower())
                    || p.City.ToString().ToLower().Contains(search.ToLower())
                    || p.CreationDate.ToString().ToLower().Contains(search.ToLower())
                    || p.ExpiryDate.ToString().ToLower().Contains(search.ToLower())
                    || p.Major.ToString().ToLower().Contains(search.ToLower())
                    || p.Street.ToString().ToLower().Contains(search.ToLower())
                    );
                }

                // Sorting.
                //data = this.SortByColumnWithOrder(order, orderDir, data);

                // Filter record count.
                int recFilter = data.Count();

                // Apply pagination.
                data = data.OrderBy(x => x.Id).Skip(startRec).Take(pageSize);

                resultList = data.ToList();
                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = resultList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }

            // Return info.
            return result;
        }

        
        [Authorize(Roles = "Admin")]
        public ActionResult getUsersOnly()
        {
            JsonResult result = new JsonResult();

            try
            {
                // Initialization.
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                // Loading.
                //SurveyModel db = new SurveyModel();
                IQueryable<RegisterViewModel> data = from usr in db.Users
                join reg in db.AspNetUsers on usr.ASPUserId equals reg.Id where(usr.isDeleted != true && reg.AspNetRoles.Any(x => x.Name == "User"))
                select new RegisterViewModel {
                    Id = usr.Id,
                    Name = usr.Name,
                    Email = reg.Email,
                    Phone = usr.Phone,
                    BirthDate = usr.BirthDate
                };

                // Total record count.
                int totalRecords = data.Count();

                // Verification.
                List<RegisterViewModel> resultList;

                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    data = data.Where(p => p.Name.ToString().ToLower().Contains(search.ToLower())
                    || p.Email.ToString().ToLower().Contains(search.ToLower())
                    || p.Phone.ToString().ToLower().Contains(search.ToLower())
                    || p.Education.ToString().ToLower().Contains(search.ToLower())
                    || p.Area.ToString().ToLower().Contains(search.ToLower())
                    || p.BirthDate.ToString().ToLower().Contains(search.ToLower())
                    || p.City.ToString().ToLower().Contains(search.ToLower())
                    || p.CreationDate.ToString().ToLower().Contains(search.ToLower())
                    || p.ExpiryDate.ToString().ToLower().Contains(search.ToLower())
                    || p.Major.ToString().ToLower().Contains(search.ToLower())
                    || p.Street.ToString().ToLower().Contains(search.ToLower())
                    );
                }

                // Sorting.
                //data = this.SortByColumnWithOrder(order, orderDir, data);

                // Filter record count.
                int recFilter = data.Count();

                // Apply pagination.
                data = data.OrderBy(x => x.Id).Skip(startRec).Take(pageSize);

                resultList = data.ToList();
                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = resultList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }

            // Return info.
            return result;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.isDeleted = true;
            db.SaveChanges();
            return RedirectToAction("UsersList");
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