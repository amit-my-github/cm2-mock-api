namespace Content.Manager.Core.WebApi.Model.Category
{
    public class CategoriesItem
    {

        public int Id { get; set; }
        public int SortingPriority { get; set; }
        public string? Value { get; set; }
        public bool SearchNumberNT { get; set; }
        public string? Notes { get; set; }
        public string? DateCreated { get; set; }
        public string? LastModified { get; set; }
        public int FacetID { get; set; }
        public string? FacetName { get; set; }
        public string? SearchNumber { get; set; }

    }

}

