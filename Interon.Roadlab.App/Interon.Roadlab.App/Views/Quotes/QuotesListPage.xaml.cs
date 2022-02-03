using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interon.Roadlab.App.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Interon.Roadlab.App.Views.Quotes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuotesListPage : ContentPage
    {
        public QuotesListPage(string type)
        {
            BindingContext = new TransactionListViewModel(type);
            InitializeComponent();
        }
        
    }
}