namespace CM.WebAPI.Model.AccessCodes
{
    public class AccessCodeItem
    {
        public int Id { get; set; }
        public string? CreatedDate { get; set; }
        public bool IsFree { get; set; }
        public string? AccessCode { get; set; }
        public string? Description { get; set; }
        public string? LastModified { get; set; }

    }

}
