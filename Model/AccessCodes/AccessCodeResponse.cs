using Content.Manager.Core.WebApi.Model.AccessCodes;
using System.Text.Json.Serialization;

namespace Content.Manager.Core.WebApi.Model.AccessCodes
{
    public class AccessCodeResponse
    {
        public AccessCodeResponse()
        {
            AccessCodes = new List<AccessCodeItem>();
        }

        [JsonInclude]
        public ICollection<AccessCodeItem> AccessCodes { get; private set; }

        public int TotalItems { get; set; }

    }
}
