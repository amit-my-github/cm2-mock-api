using Content.Manager.Core.WebApi.Model;
using Newtonsoft.Json;

namespace Asp.Net.Core.WebApi.CrudOperations.Services
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
                User = JsonConvert.DeserializeObject<UserItem>(json);

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
