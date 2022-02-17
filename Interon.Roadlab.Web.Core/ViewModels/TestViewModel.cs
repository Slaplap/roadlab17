using Interon.Roadlab.Web.Core.Interfaces;
using Interon.Roadlab.Web.Core.Models;

namespace Interon.Roadlab.Web.Core.ViewModels
{
    public class TestViewModel:IMapFrom<TestModel>
    {
        public string Name { get; set; }    
    }
}