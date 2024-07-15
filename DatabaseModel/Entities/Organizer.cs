namespace DatabaseModel.Entities
{
    public class Organizer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string? OrganizerName { get; set; }
    }
}
