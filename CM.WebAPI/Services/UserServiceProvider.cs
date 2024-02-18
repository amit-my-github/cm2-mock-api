using CM.WebAPI.Model;
using System.Text.Json;

namespace CM.WebAPI.Services
{
    public static class UserServiceProvider
    {
        #region Properties  
        static UserItem? User { get; }

        #endregion

        #region Constructor
        static UserServiceProvider()
        {

            string currentDirectory = Directory.GetCurrentDirectory();
            string path = "json";
            string fullPath = Path.Combine(currentDirectory, path, "user.json");
            using (StreamReader r = new StreamReader(fullPath))
            {
                string json = r.ReadToEnd();
                User = JsonSerializer.Deserialize<UserItem>(json);

            }
        }

        #endregion

        #region Methods

        public static UserItem? GetAll()
        {
            if (User != null)
            {
                return User;
            }
            else { return null; }
        }

        #endregion
    }
}
