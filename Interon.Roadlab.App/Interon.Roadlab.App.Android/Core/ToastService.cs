using System;
using Android.Content;
using Android.Widget;
using Interon.Roadlab.App.Droid.Core;
using Interon.Roadlab.Core.Enums;
using Interon.Roadlab.Core.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(ToastService))]
namespace Interon.Roadlab.App.Droid.Core
{
    public class ToastService : IToastService
    {
        internal static Func<Context> GetContext { get; set; }

        public void CookIt(string message, MyToastLength length)
        {
            var context = GetContext();
            var toastLength = (length == MyToastLength.Long) ? ToastLength.Long : ToastLength.Short;
            Toast.MakeText(context, message, toastLength).Show();
        }

    }
}