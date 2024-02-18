using CM.WebAPI.Model.Token;
using System.Text.Json;

namespace CM.WebAPI.Services
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
                Token = JsonSerializer.Deserialize<TokenItem>(json);

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
