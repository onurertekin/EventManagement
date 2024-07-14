namespace DatabaseModel.Entities
{
    public class Participant
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RegisteredDate { get; set; }

        #region Navigation Properties

        public virtual Event Event { get; set; }

        #endregion
    }
}
