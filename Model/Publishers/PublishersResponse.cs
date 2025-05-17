using Content.Manager.Core.WebApi.Model.Publishers;
using System.Text.Json.Serialization;

namespace Content.Manager.Core.WebApi.Model.Publishers
{
    public class PublishersResponse
    {

        public PublishersResponse()
        {
            Publishers = new List<PublisherItem>();
        }

        [JsonInclude]
        public ICollection<PublisherItem> Publishers { get; private set; }

        public int TotalItems { get; set; }

    }
}
