using System.Collections.Generic;
using Interon.Roadlab.App.Core.Models;

namespace Interon.Roadlab.App.Core
{
    public sealed class Context
    {
        Context()
        {
        }
        private static readonly object padlock = new object();
        private static Context instance = null;
        public static Context Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Context();
                    }
                    return instance;
                }
            }
        }

        public bool IsBranchUpdated { get; set; }
        public  IEnumerable<Branch> Branches { get; set; } = new List<Branch>();
        public bool IsLocal { get; set; }  
        public bool IsRunning { get; set; }
    }
}
