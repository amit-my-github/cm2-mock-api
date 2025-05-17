using System.Text.Json.Serialization;

namespace Content.Manager.Core.WebApi.Model.Facet
{
    public class FacetsResponse
    {
        public FacetsResponse()
        {
            Facets = new List<FacetsItem>();
        }

        [JsonInclude]
        public ICollection<FacetsItem> Facets { get; private set; }

        public int TotalItems { get; set; }

    }
}
