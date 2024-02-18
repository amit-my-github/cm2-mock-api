using System.Text.Json.Serialization;

namespace CM.WebAPI.Model.Category
{
    public class CategoriesResponse
    {
        public CategoriesResponse()
        {
            Categories = new List<CategoriesItem>();
        }

        [JsonInclude]
        public ICollection<CategoriesItem> Categories { get; private set; }

        public int TotalItems { get; set; }

    }
}
