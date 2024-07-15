namespace Contract.Request.Events
{
    public class CreateEventRequest
    {
        public int organizerId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

    }
}
