
using System.Text.Json;

namespace CM.WebAPI.Model.Publication
{
    public class Categories
    {
        public CategorieItem? Instantie { get; set; }

        public ICollection<CategorieItem>? LawAreas { get; set; }
    }

    public class CategorieItem
    {
        public int CategoryId { get; set; }

        public string? Value { get; set; }
    }
        

    public class PublicationTitleRelations
    {
        public RelationItem? PartOf { get; set; }

        public ICollection<RelationItem>? TitleOf { get; set; }

        public ICollection<RelationItem>? GivesAccessTo { get; set; }

        public ICollection<RelationItem>? AccessedVia { get; set; }
    }

    public class PublicationTitleMappingAndRelationResponse
    {
        public Title? Title { get; set; }

        public Categories? Categories { get; set; }

        public PublicationTitleRelations? PublicationTitleRelations { get; set; }
    }

    public class Title
    {
        public int PublicationTitleId { get; set; }

        public string? DisplayCode { get; set; }

        public string? Type { get; set; }

        public string? DateCreated { get; set; }

        public string? LastModified { get; set; }

        public string? AccessCode { get; set; }

        public ICollection<string>? ExternalCode { get; set; }

        public ICollection<string>? ThesaurusSynonyms { get; set; }

        public string? Name { get; set; }

        public string? Notes { get; set; }
    }

    public class RelationItem
    {
        public int PublicationTitlesId { get; set; }

        public string? DisplayCode { get; set; }

        public string? Type { get; set; }

        public string? AccessCode { get; set; }
    }


}


