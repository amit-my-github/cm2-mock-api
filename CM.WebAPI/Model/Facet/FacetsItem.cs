namespace CM.WebAPI.Model.Facet
{
    public class FacetsItem
    {
        public int Id { get; set; }
        public string? FacetName { get; set; }

#pragma warning disable VSSpell001 // Spell Check
        public string? SolrField { get; set; }
#pragma warning restore VSSpell001 // Spell Check

        public string? Description { get; set; }


    }

}
