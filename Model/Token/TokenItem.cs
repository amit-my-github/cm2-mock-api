namespace Content.Manager.Core.WebApi.Model.Token
{
    public class TokenItem
    {
        public string? AccessToken { get; set; }

        public string? UserName { get; set; }

        public int ExpiresIn { get; set; }

        public string? Role { get; set; }

    }

}
