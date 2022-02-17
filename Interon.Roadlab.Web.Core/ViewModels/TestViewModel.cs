using Interon.Roadlab.Web.App.Interfaces;
using Interon.Roadlab.Web.App.Models;

namespace Interon.Roadlab.Web.App.ViewModels
{
    public class TestViewModel:IMapFrom<TestModel>
    {
        public string Name { get; set; }    
    }
}