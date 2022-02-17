using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Interon.Roadlab.Web.App.Interfaces;
using Interon.Roadlab.Web.App.Services;
using Interon.Roadlab.Web.App.ViewModels;
using Interon.Roadlab.Web.Core.ContentModels;
using Umbraco.Core.Cache;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedModels;

namespace Interon.Roadlab.Web.App.SurfaceControllers
{
    public class MembershipController : SurfaceController
    {
        private readonly ServiceContext _services;
     
        private IMembershipService _membershipService;
        private UmbracoHelper _umbracoHelper;

        public MembershipController(IUmbracoContextAccessor umbracoContextAccessor,
            IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, ILogger logger,
            IProfilingLogger profilingLogger, UmbracoHelper umbracoHelper, IMembershipService membershipService) : base(umbracoContextAccessor,
            databaseFactory, services, appCaches, logger, profilingLogger, umbracoHelper)
        {
            _services = services;
         
            _membershipService = membershipService;
            _umbracoHelper = umbracoHelper;
        }

        [HttpGet]
        [ActionName("RenderLogin")]
        public ActionResult RenderLogin()
        {
            return PartialView("_RenderLoginForm", new LoginViewModel());
        }

        [HttpGet]
        [ActionName("Logout")]
        public ActionResult Logout()
        {
            TempData.Clear();
            Session.Clear();
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        [HttpPost]
        [ActionName("Validate")]
        [ValidateAntiForgeryToken]
        public ActionResult Validate(LoginViewModel model)
        {
            if (!ModelState.IsValid) return CurrentUmbracoPage();
            model.Login = model.Login.Trim().ToLower();
            if (Membership.ValidateUser(model.Login, model.Password))
            {
                FormsAuthentication.SetAuthCookie(model.Login, model.RememberMe);
                if (!string.IsNullOrWhiteSpace(TempData["RedirectUrl"].ToString())&& TempData["RedirectUrl"].ToString().ToLower().Contains("create"))
                {
                    return Redirect(TempData["RedirectUrl"].ToString());
                }
                else
                {

                    return ThisRedirect(AccountHome.ModelTypeAlias);
                }

            }

            TempData["Status"] = "Invalid Log-in Credentials";
            return RedirectToCurrentUmbracoPage();
        }

        private ActionResult ThisRedirect(string modelTypeAlias)
        {
            return RedirectToUmbracoPage(Umbraco.ContentAtRoot().FirstOrDefault().DescendantOfType(modelTypeAlias));
        }

        [HttpGet]
        [ActionName("Register")]
        public ActionResult Register()
        {
            return PartialView("_RenderRegistrationForm", new RegistrationViewModel());
        }

        [HttpPost]
        [ActionName("ChangePassword")]
        public ActionResult ChangePassword(PasswordChangeViewModel model)
        {
            _membershipService.ChangePassword(Members.GetCurrentMember().Id, model.Password);
            return ThisRedirect(AccountHome.ModelTypeAlias);

        }


        [HttpPost]
        [ActionName("Registration")]
        public async Task<ActionResult> Registration(RegistrationViewModel model)
        {
            model.Login = model.Login.Trim().ToLower();

            if (!ModelState.IsValid) return CurrentUmbracoPage();
            //HoneyPot
            if (!string.IsNullOrWhiteSpace(model.RepeatPassword))
            {
                return RedirectToRoute("/");
            }


            if (_membershipService.GetByEmail(model.Login) != null)
            {
                ModelState.AddModelError("", "The user already exists");
                return CurrentUmbracoPage();
            }

            var company = await LIMS.Services.ClientsAndContactsService.ClientByAccountNumberAsync(model.AccountNumber);


            if (company == null)
            {
                ModelState.AddModelError("", "Account not found");
                return CurrentUmbracoPage();
            }

            var member = _membershipService.CreateMember(model.Login, model.Name, model.Surname, model.CellNumber, model.AccountNumber, model.Password, company);
            if (member == null || member.Id == 0)
            {
                ModelState.AddModelError("", "Error Creating Member");
            }
            if (Membership.ValidateUser(model.Login, model.Password))
            {
                FormsAuthentication.SetAuthCookie(model.Login, false);
                return ThisRedirect(AccountProfile.ModelTypeAlias);
            }

            ModelState.AddModelError("", "Invalid Log-in Credentials");
            return CurrentUmbracoPage();
        }

        [HttpPost]
        [ActionName("Reset")]
        [ValidateAntiForgeryToken]
        public ActionResult Reset(PasswordResetViewModel model)
        {
            model.Email = model.Email.Trim().ToLower();
            if (!ModelState.IsValid) return CurrentUmbracoPage();

            var _membershipService = _services.MemberService;
            var _contentService = _services.ContentService;
            var member = _membershipService.GetByEmail(model.Email);
            var _member = (Member)_umbracoHelper.Member(member.Id);
            if (_member == null)
            {
              
                return CurrentUmbracoPage();
            }


            if (_member.Otp.Trim() != model.OTP.Trim())
            {
                return CurrentUmbracoPage();
            }
            _membershipService.SavePassword(member, model.Password);
            _membershipService.Save(member);

            if (Membership.ValidateUser(model.Email, model.Password))
                FormsAuthentication.SetAuthCookie(model.Email, true);
            return ThisRedirect(AccountHome.ModelTypeAlias);

          
            return CurrentUmbracoPage();
        }
    }
}