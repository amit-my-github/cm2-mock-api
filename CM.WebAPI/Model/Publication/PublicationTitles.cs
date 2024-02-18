namespace CM.WebAPI.Model.Publication
{
    public class PublicationTitles
    {
        public string? PublisherName { get; set; }
        public ICollection<PublicationTitle>? Titles { get; set; }
    }

}
