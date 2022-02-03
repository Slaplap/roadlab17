using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Interon.Roadlab.App.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Rockface : ContentView
    {
        public static readonly BindableProperty ShowProperty = BindableProperty.Create(
            "Show",        // the name of the bindable property
            typeof(bool),     // the bindable property type
            typeof(Rockface),   // the parent object type
            false);      //

        public bool Show
        {
            get => (bool)GetValue(Rockface.ShowProperty);
            set => SetValue(Rockface.ShowProperty, value);
        }
        public static readonly BindableProperty ShowIconProperty = BindableProperty.Create(
            "ShowIcon",        // the name of the bindable property
            typeof(bool),     // the bindable property type
            typeof(Rockface),   // the parent object type
            false);      //

        public bool ShowIcon
        {
            get => (bool)GetValue(Rockface.ShowIconProperty);
            set => SetValue(Rockface.ShowIconProperty, value);
        }

        public Rockface()
        {
            InitializeComponent();
            var b = Shell.Current.FlyoutBehavior == FlyoutBehavior.Disabled;
            Shell.Current.FlyoutIcon = new StreamImageSource();

            //  var s = Shell.Current.CurrentState.Location.AbsoluteUri;

            //  Shell.SetNavBarShow(null,false);
        }

        private void OpenFlyout(object sender, EventArgs e)
        {
            Shell.Current.FlyoutIsPresented = true;
        }

        private void PushBack(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PopAsync();

        }
    }
}