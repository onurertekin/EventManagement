namespace Contract.Response.Participant
{
    public class SearchParticipantResponse
    {
        public class Participants
        {
            public int id { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string email { get; set; }
            public string password { get; set; }
            public string phoneNumber { get; set; }
            public DateTime registeredDate { get; set; }
        }
        public SearchParticipantResponse()
        {
            participants = new List<Participants>();
        }
        public List<Participants> participants { get; set; }
    }
}
