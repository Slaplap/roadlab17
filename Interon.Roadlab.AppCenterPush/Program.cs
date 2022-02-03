using System;
using System.Collections.Generic;

namespace Interon.Roadlab.AppCenterPush
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            
            var content = new Core.AppCenterPush.Content();
            content.Title = "Test from Roadlab";
            content.Body = "this is the payload";
            content.Name = "Test";
          //  content.CustomData.Add("Foo","Bar");

            var recipients = new Dictionary<Guid, string>();
            recipients.Add(Guid.Parse("75fd2584-9d05-49a0-9c14-c1367a0098b0"), "Android"); ;
            var acp = new Core.AppCenterPush(recipients);
            acp.Notify(content.Title, content.Body, null).ConfigureAwait(true);

        }
    }
}