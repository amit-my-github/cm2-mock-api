namespace CM.WebAPI.Model.Parameters
{
    public class PublicationQueryParameters : QueryParameters
    {
        public string? Type { get; set; }

        public bool? Feed { get; set; }

        public bool? IsActive { get; set; }

        public string? Field { get; set; }

        public string? ExtCode { get; set; }

        public string? Status { get; set; }

    }

}
