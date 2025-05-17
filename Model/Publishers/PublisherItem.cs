namespace Content.Manager.Core.WebApi.Model.Publishers
{
    public class PublisherItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
        public string? LastModified { get; set; }
        public string? DateCreated { get; set; }
        public string? Notes { get; set; }
        public int TotalRows { get; set; }

    }
}
