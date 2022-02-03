using SQLite;

namespace Interon.Roadlab.App.Core.Services
{
    public class BaseService
    {
        private static SQLiteConnection privatecon;

        public static SQLiteConnection GetConnection()
        {
            if (privatecon == null)
            {
                string devicePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                var path = System.IO.Path.Combine(devicePath, "roadlab.db3");
                privatecon = new SQLiteConnection(path);
            }

            return privatecon;
        }
    }
}