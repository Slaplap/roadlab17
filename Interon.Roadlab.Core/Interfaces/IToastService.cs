using Interon.Roadlab.Core.Enums;

namespace Interon.Roadlab.Core.Interfaces
{
    public interface IToastService
    {
        void CookIt(string message, MyToastLength length);
    }
}
