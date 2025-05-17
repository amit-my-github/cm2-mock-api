namespace Content.Manager.Core.WebApi.Model.Publication
{
    public class PublicationTitles
    {
        public string? PublisherName { get; set; }
        public ICollection<PublicationTitle>? Titles { get; set; }
    }

}
