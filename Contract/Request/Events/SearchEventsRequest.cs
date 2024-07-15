namespace Contract.Request.Events
{
    public class SearchEventsRequest
    {
        public int id { get; set; }
        public int organizerId { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public string? location { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime updatedOn { get; set; }

    }
}
