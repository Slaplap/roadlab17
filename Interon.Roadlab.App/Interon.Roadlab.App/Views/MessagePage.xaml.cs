using Interon.Roadlab.App.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Interon.Roadlab.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    
    [QueryProperty("MessageType", "MessageType")]
    public partial class MessagePage : ContentPage
    {
      
        private string _messageType;

        public MessagePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = new MessageViewModel(MessageType);
        }

        

        public string MessageType
        {
            get => _messageType;
            set => _messageType = value;
        }
    }

    
}