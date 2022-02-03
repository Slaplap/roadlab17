using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interon.Roadlab.App.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Interon.Roadlab.App.Views.Orders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrdersListPage : ContentPage
    {
        public OrdersListPage()
        {
            BindingContext = new OrdersListViewModel();
            InitializeComponent();
        }
    }
}