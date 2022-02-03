using Xamarin.Forms;

namespace Interon.Roadlab.App.Core.ViewModels
{
    public class ModalMessageViewModel : ViewModelBase
    {
        public ModalMessageViewModel()
        {
            Commands.Add("Close", new Command(CmdClose));
        }

        private void CmdClose()
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private string _message;

        public string Message
        {
            get => _message;
            set
            {
             
                SetProperty(ref _message,value);
            }
        }
    }
}