using Content.Manager.Core.WebApi.Model.Token;
using Newtonsoft.Json;

namespace Asp.Net.Core.WebApi.CrudOperations.Services
{
    public static class TokenServiceProvider
    {
        #region Properties  
        static TokenItem? Token { get; }

        #endregion

        #region Constructor
        static TokenServiceProvider()
        {

            string currentDirectory = Directory.GetCurrentDirectory();
            string path = "json";
            string fullPath = Path.Combine(currentDirectory, path, "token.json");
            using (StreamReader r = new StreamReader(fullPath))
            {
                string json = r.ReadToEnd();
                Token = JsonConvert.DeserializeObject<TokenItem>(json);

            }
        }

        #endregion

        #region Methods

        public static TokenItem? GetAll()
        {
            if (Token != null)
            {
                return Token;
            }
            else { return null; }
        }

        #endregion
    }
}
