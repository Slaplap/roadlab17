using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Interon.Roadlab.App.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Header : ContentView
    {
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
            "Title",        // the name of the bindable property
            typeof(string),     // the bindable property type
            typeof(Header),   // the parent object type
            string.Empty);      //

        public string Title
        {
            get => (string)GetValue(Header.TitleProperty);
            set => SetValue(Header.TitleProperty, value);
        }

        public static readonly BindableProperty ShowDeepProperty = BindableProperty.Create(
            "ShowDeep",        // the name of the bindable property
            typeof(bool),     // the bindable property type
            typeof(Header),   // the parent object type
            false);      //

        public bool ShowDeep
        {
            get => (bool)GetValue(Header.ShowDeepProperty);
            set => SetValue(Header.ShowDeepProperty, value);
        }
        

        public Header()
        {
            InitializeComponent();
        }
    }
}