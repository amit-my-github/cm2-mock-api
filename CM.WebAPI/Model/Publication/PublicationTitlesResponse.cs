using System.Text.Json.Serialization;

namespace CM.WebAPI.Model.Publication
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
