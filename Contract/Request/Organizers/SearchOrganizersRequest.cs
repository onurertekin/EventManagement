namespace Contract.Request.Organizers
{
    public class SearchOrganizersRequest
    {
        public int id { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public string? phoneNumber { get; set; }
        public string? organizerName { get; set; }
    }
}
