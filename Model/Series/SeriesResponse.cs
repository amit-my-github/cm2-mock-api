namespace Content.Manager.Core.WebApi.Model.Series
{
    public class SeriesResponse
    {
        public SeriesResponse()
        {

            SeriesItems = new List<SeriesItem>();
        }

        public ICollection<SeriesItem> SeriesItems { get; private set; }
        public int TotalItems { get; set; }
    }
}
