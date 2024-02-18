namespace CM.WebAPI.Model.AccessCodes
{
    public class AddAccessCodeRequest
    {
        public bool IsFree { get; set; }
        public string? AccessCode { get; set; }
        public string? Description { get; set; }

    }

}
