using System.Collections.Generic;

namespace Interon.Roadlab.Core.Interfaces
{
    public interface INotificationHub
    {
         
            void UpdateTags(string ID, List<string> gr);    
            string ID { get; }
         
    }
}
