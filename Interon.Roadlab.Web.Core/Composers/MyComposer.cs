using ClientDependency.Core;
using ClientDependency.Core.Logging;
using Interon.Roadlab.Web.App.Interfaces;
using Interon.Roadlab.Web.App.Migrations;
using Interon.Roadlab.Web.App.Services;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Events;

namespace Interon.Roadlab.Web.App.Composers
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class MyComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
          

            composition.Components().Append<MyComponent>();
            composition.Components().Append<MigrationComponent>();
            //composition.Register<TransactionService>();
            composition.Register<NotificationService>();
            composition.Register<MessageService>();
           
            composition.Register<IMembershipService,MembershipService>(Lifetime.Request);
            CreateBundles();
        }

        public static void CreateBundles()
        {
            BundleManager.CreateCssBundle("main",
                new CssFile("https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css"),
                new CssFile("https://fonts.googleapis.com/css?family=Barlow:200,200i,300,300i,500,500i,700,800&display=swap"),
                new CssFile("~/css/style.css"),
                new CssFile("~/css/animations.css"),
                new CssFile("~/css/toast.css"),
                new CssFile("~/css/animate.css") 
                 
            );
             
          

        BundleManager.CreateJsBundle("main",
                new JavascriptFile("https://kit.fontawesome.com/159299d96f.js"),

                new JavascriptFile("https://cdnjs.cloudflare.com/ajax/libs/jquery/3.4.1/jquery.min.js"),
                new JavascriptFile("https://ajax.aspnetcdn.com/ajax/jquery.validate/1.16.0/jquery.validate.min.js"),
                new JavascriptFile("https://ajax.aspnetcdn.com/ajax/mvc/5.2.3/jquery.validate.unobtrusive.min.js"),
                new JavascriptFile("https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"),
                new JavascriptFile("https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"),
                new JavascriptFile("~/js/jquery.themepunch.tools.min.js"),
                new JavascriptFile("~/js/jquery.themepunch.revolution.min.js"),
                new JavascriptFile("~/js/animations.js"),
                new JavascriptFile("~/js/toast.js"),
                new JavascriptFile("~/js/roadlab.js")
         





            );
        }
    }
}