using System;
using System.Linq;
using System.Threading.Tasks;
using Interon.Roadlab.App.Core.Services;
using Interon.Roadlab.Core.Enums;
using Interon.Roadlab.Core.Json;
using Newtonsoft.Json;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace Interon.Roadlab.App.Core.ViewModels
{
    
    class ClientRequestCommentsViewModel : ViewModelBase
    {
        private readonly int _id;
        private string _comment;
        private Guid _transactionKey;
        private string _requestType;
        private string _heading;
        private bool _errorVisible;

        public ClientRequestCommentsViewModel(int  id,string requestType )
        {
            _id = id;
            _requestType = requestType;
            this.IsBusy = false;
            Commands.Add("CommandCancel", new AsyncCommand(CommandCancel, allowsMultipleExecutions: false));
            Commands.Add("CommandSubmit", new Command(CommandSubmit));
            Heading = "Rejection Reason";

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

        public Guid transactionKey
        {
            get => _transactionKey;
            set => SetProperty(ref _transactionKey, value);
        }

        public string transactionType
        {
            get => _requestType;
            set => SetProperty(ref _requestType, value);
        }

        public bool ErrorVisible
        {
            get => _errorVisible;
            set => SetProperty(ref _errorVisible, value);
        }

        private  void CommandSubmit()
        {
            if (string.IsNullOrEmpty(Comment))
            {
                ErrorVisible = true;
                return;
            }
          
                Comments comments = new Comments();
                comments.CommentsList.Add(new Comment()
                {
                    DateTime =  DateTime.Now,
                    Message = Comment,
                    Name = SecureStorageService.Username
                });
                RequestService requestService = new RequestService();
                var transaction = requestService.GetClientRequestById( _id);
               //todo impliment
               // transaction.Status = ClientRequestStatus.GetValue(transaction.Status.FirstOrDefault().Split(' ')[0], ClientRequestStatus.Rejected);
                
                MessagingCenter.Send<object>(this, "RequestChange");
                Shell.Current.Navigation.PopAsync();

        }

        private async Task CommandCancel()
        {
          await  Shell.Current.Navigation.PopToRootAsync();

        }
    }
}
