using DatabaseModel.Entities;

namespace Contract.Response
{
    public class SearchOrganizersResponse
    {
        public int id { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? email { get; set; }
        public string? phoneNumber { get; set; }
        public string? organizerName { get; set; }

        public SearchOrganizersResponse()
        {
            organizers = new List<Organizer>();
        }
        public List<Organizer> organizers { get; set; }
    }
}
