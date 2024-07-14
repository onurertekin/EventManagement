namespace Contract.Response.Organizers
{
    public class SearchOrganizersResponse
    {
        public class Organizers
        {
            public int id { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string email { get; set; }
            public string phoneNumber { get; set; }
            public string organizerName { get; set; }
        }

        public SearchOrganizersResponse()
        {
            organizers = new List<Organizers>();
        }
        public List<Organizers> organizers { get; set; }
    }
}
