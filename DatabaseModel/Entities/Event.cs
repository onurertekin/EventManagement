namespace DatabaseModel.Entities
{
    public class Event
    {
        public Event()
        {
            Participants = new HashSet<Participant>();
        }

        public int Id { get; set; }
        public int OrganizerId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Location { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        #region Navigation Properties

        public virtual Organizer Organizer { get; set; }
        public virtual ISet<Participant> Participants { get; set; }
        #endregion
    }
}