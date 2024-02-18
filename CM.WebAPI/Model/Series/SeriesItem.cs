namespace CM.WebAPI.Model.Series
{
    public class SeriesItem
    {
        public int Id { get; set; }
        public string? Series { get; set; }

        public string? Publisher { get; set; }
        public int SortingPriority { get; set; }

    }
}
