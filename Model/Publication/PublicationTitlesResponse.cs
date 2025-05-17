using System.Text.Json.Serialization;

namespace Content.Manager.Core.WebApi.Model.Publication
{
    public class PublicationTitlesResponse
    {
        public PublicationTitlesResponse()
        {
            PublicationTitles = new List<PublicationTitles>();
        }

        [JsonInclude]
        public ICollection<PublicationTitles>? PublicationTitles { get; private set; }

        public int TotalItems { get; set; }
    }

}
