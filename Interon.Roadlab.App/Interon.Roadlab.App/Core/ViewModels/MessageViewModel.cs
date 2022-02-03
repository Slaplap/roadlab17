using System;
using System.Threading.Tasks;
using Interon.Roadlab.App.Core.Services;
using Interon.Roadlab.App.Views;
using Interon.Roadlab.Core.Dto;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace Interon.Roadlab.App.Core.ViewModels
{

    class MessageViewModel : ViewModelBase
    {
        private readonly string _key;
        private string _comment;
        private string _messageType;
        private string _heading;
        private bool _errorVisible;
        private string _description;

        public MessageViewModel(string messageType)
        {

            _messageType = messageType;
            this.IsBusy = false;
            Commands.Add("CommandCancel", new AsyncCommand(CommandCancel, allowsMultipleExecutions: false));
            Commands.Add("CommandSubmit", new AsyncCommand(CommandSubmit, allowsMultipleExecutions: false));
            if (messageType == "feedback")
            {
                Heading = "Feedback";
                Description = "Please let us know what you think of our application. Also feel free to submit your recommendations on how to improve or what features to add to the app;";
            }
            if (messageType == "callback")
            {
                Heading = "Call Back";
                Description = "Leave a short message and one of our consultants will get back to you";
            }

        }

        

        public string Heading
        {
            get => _heading;
            set => SetProperty(ref _heading, value);
        }
        public string Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }


        public string messageType
        {
            get => _messageType;
            set => SetProperty(ref _messageType, value);
        }

        public bool ErrorVisible
        {
            get => _errorVisible;
            set => SetProperty(ref _errorVisible, value);
        }

        private async Task CommandSubmit()
        {
            if (string.IsNullOrEmpty(Comment))
            {
                ErrorVisible = true;
                return;
            }

            try
            {
               await MessageOnlineService.SaveMessageJsonDto(new MessageJsonDto()
                {
                    CreateDate = DateTime.Now,
                    Key = Guid.NewGuid(),
                    MemberId = SecureStorageService.MemberId,
                    MessageType = _messageType,
                    Payload = Comment,
                    SyncDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                });
                await Shell.Current.Navigation.PushModalAsync(new InfoPage(new ModalMessageViewModel()
                {
                    Message = "Thank you for your request."
                }), true);
            }
            catch (Exception e)
            {
               await  Shell.Current.Navigation.PushModalAsync(new ErrorPage(new ErrorMessageViewModel()
                {
                    Message = e.Message
                }), true);
            }


        }

        private async Task CommandCancel()
        {
            await Shell.Current.Navigation.PopToRootAsync();

        }
    }
}
