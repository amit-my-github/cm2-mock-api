namespace Content.Manager.Core.WebApi.Model.Publication
{
    public class PublicationTitleItem
    {

        public string? Type { get; set; }
        public string? Name { get; set; }
        public string? DisplayCode { get; set; }
        public int AccessCodeId { get; set; }
        public bool IsExcludeFromAllInSubscription { get; set; }
        public bool IsDisplayOnAccessRightsPage { get; set; }
        public string? Status { get; set; }
        public string? Isbn { get; set; }
        public ICollection<ExternalCode>? ExternalCodes { get; set; }
        public bool IsPowerFilter { get; set; }
        public ICollection<string>? Thesaurus { get; set; }
        public int MonitorLimit { get; set; }
        public bool IsReference { get; set; }
        public bool IsOverAllTitle { get; set; }
        public bool HasParts { get; set; }
        public int? PartOf { get; set; }
        public string? FeedUrl { get; set; }
        public string? ContentsUrl { get; set; }
        public bool InSourcePage { get; set; }
        public string? PublicationStart { get; set; }

        public string? EmbargoDate { get; set; }

        public string? PublicationEnd { get; set; }

        public string? PubAvailabilityInBasicService { get; set; }

        public string? PublicationOrderInformation { get; set; }

        public string? PublicationSpecialFeatures { get; set; }

        public string? PublicationDescription { get; set; }

        public string? Notes { get; set; }

        public string? DateCreated { get; set; }

        public string? LastModified { get; set; }
        public int PublisherID { get; set; }
        public int Id { get; set; }

        public string? AccessCode { get; set; }


    }

}

