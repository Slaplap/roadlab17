using System.Threading.Tasks;
using Xamarin.Forms;

public static class NavigationExtensions
{
    public static async Task PopAllModals(this INavigation navigation)
    {

        foreach (var page in navigation.ModalStack)
        {
            await navigation.PopModalAsync(false);
        }
    }
}