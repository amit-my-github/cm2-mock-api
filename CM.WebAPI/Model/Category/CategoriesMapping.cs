namespace CM.WebAPI.Model.Category
{
    public class CategoriesMapping
    {
        public int Id { get; set; }
        public int FacetId { get; set; }
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public string? LastModified { get; set; }

    }

    public class CategoriesMappingResponse
    {
        public int TotalItems { get; set; }
        public List<CategoriesMapping>? CategoriesMapping { get; set; }
    }


}

