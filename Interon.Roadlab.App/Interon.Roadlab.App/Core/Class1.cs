using System;
using System.Windows.Input;

namespace Interon.Roadlab.App.Core
{
     
     
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}
