using System.Text.Json.Serialization;
using Content.Manager.Core.WebApi.Model.Category;

namespace Content.Manager.Core.WebApi.Model.Publication
{
    public class PublicationTitleResponse
    {
        public PublicationTitleResponse()
        {
            Categories = new List<Categories>();
            TitleOf = new List<HasPartOf>();

        }

        [JsonInclude]
        public PublicationTitle? Title { get; set; }
        public ICollection<Categories>? Categories { get; private set; }
        public HasPartOf? PartOf { get; set; }
        public ICollection<HasPartOf>? TitleOf { get; private set; }

    }
}


