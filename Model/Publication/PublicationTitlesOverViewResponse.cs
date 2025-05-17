
namespace Content.Manager.Core.WebApi.Model.Publication
{
    public class PublicationTitlesOverViewResponse
    {
        public PublicationTitlesOverViewResponse()
        {
            PublicationTitles = new List<PublicationTitlesOverView>();
        }

        public ICollection<PublicationTitlesOverView>? PublicationTitles { get; set; }

        public int TotalItems { get; set; }

    }

    public class PublicationTitleOverView
    {
        public int Id { get; set; }
        public string? DisplayCode { get; set; }
        public string? DateCreated { get; set; }
        public string? Status { get; set; }
        public string? DateModified { get; set; }
        public int Usage { get; set; }
        public string? Type { get; set; }
        public ICollection<string>? ExternalCode { get; set; }

    }
    public class PublicationTitlesOverView
    {
        public string? PublisherName { get; set; }
        public ICollection<PublicationTitleOverView>? Titles { get; set; }
    }

}


