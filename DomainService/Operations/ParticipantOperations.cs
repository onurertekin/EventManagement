using DatabaseModel;
using DatabaseModel.Entities;

namespace DomainService.Operations
{
    public class ParticipantOperations
    {
        private MainDbContext mainDbContext;
        public ParticipantOperations(MainDbContext mainDbContext)
        {
            this.mainDbContext = mainDbContext;
        }

        public IList<Participant> Search(int eventId, string firstName, string lastName, string email, string phoneNumber, DateTime registeredDate, string password)
        {
            var query = mainDbContext.Participants.AsQueryable();

            if (eventId != null && eventId != 0)
                query = mainDbContext.Participants.Where(x => x.EventId == eventId);
            if (!string.IsNullOrEmpty(firstName))
                query = mainDbContext.Participants.Where(x => x.FirstName == firstName);
            if (!string.IsNullOrEmpty(lastName))
                query = mainDbContext.Participants.Where(x => x.LastName == lastName);
            if (!string.IsNullOrEmpty(email))
                query = mainDbContext.Participants.Where(x => x.Email == email);
            if (!string.IsNullOrEmpty(phoneNumber))
                query = mainDbContext.Participants.Where(x => x.PhoneNumber == phoneNumber);
            if (registeredDate != default(DateTime))
                query = mainDbContext.Participants.Where(x => x.RegisteredDate == registeredDate);
            if (!string.IsNullOrEmpty(password))
                query = mainDbContext.Participants.Where(x => x.Password == password);

            return query.ToList();
        }

        public Participant GetSingle(int id)
        {
            var participant = mainDbContext.Participants.Where(x => x.Id == id).SingleOrDefault();
            if (participant == null)
                throw new Exception("Böyle bir katılımcı bulunamadı.");

            return participant;
        }

        public void Create(int eventId, string firstName, string lastName, string email, string phoneNumber, DateTime registeredDate,string password)
        {
            #region Validations

            var currentlyEvent = mainDbContext.Events.Where(x => x.Id == eventId).SingleOrDefault();
            if (currentlyEvent == null)
                throw new Exception("Böyle bir event bulunamadı.");
            var currentlyEmail = mainDbContext.Participants.Where(x => x.Email == email).SingleOrDefault();
            if (currentlyEmail != null)
                throw new Exception("Böyle bir email zaten kayıtlı.");
            var currentlyPhoneNumber = mainDbContext.Participants.Where(x => x.PhoneNumber == phoneNumber).SingleOrDefault();
            if (currentlyPhoneNumber != null)
                throw new Exception("Böyle bir telefon numarası zaten kayıtlı");

            #endregion

            var participant = new Participant();
            participant.EventId = eventId;
            participant.FirstName = firstName;
            participant.LastName = lastName;
            participant.Email = email;
            participant.PhoneNumber = phoneNumber;
            participant.RegisteredDate = registeredDate;
            participant.Password = password;

            mainDbContext.Participants.Add(participant);
            mainDbContext.SaveChanges();
        }

        public void Update(int id, int eventId, string firstName, string lastName, string email, string phoneNumber, DateTime registeredDate,string password)
        {
            #region Validations
            var participant = mainDbContext.Participants.Where(x => x.Id == id).SingleOrDefault();
            if (participant == null)
                throw new Exception("Güncellenicek bir katılımcı bulunamadı.");
            var currentlyEvent = mainDbContext.Events.Where(x => x.Id == eventId).SingleOrDefault();
            if (currentlyEvent == null)
                throw new Exception("Böyle bir event bulunamadı.");
            var currentlyEmail = mainDbContext.Participants.Where(x => x.Email == email).SingleOrDefault();
            if (currentlyEmail != null)
                throw new Exception("Böyle bir email zaten kayıtlı.");
            var currentlyPhoneNumber = mainDbContext.Participants.Where(x => x.PhoneNumber == phoneNumber).SingleOrDefault();
            if (currentlyPhoneNumber != null)
                throw new Exception("Böyle bir telefon numarası zaten kayıtlı");

            #endregion
            participant.EventId = eventId;
            participant.FirstName = firstName;
            participant.LastName = lastName;
            participant.Email = email;
            participant.PhoneNumber = phoneNumber;
            participant.RegisteredDate = registeredDate;
            participant.Password = password;

            mainDbContext.SaveChanges();

        }

        public void Delete(int id)
        {
            var participant = mainDbContext.Participants.Where(x => x.Id == id).SingleOrDefault();
            if (participant == null)
                throw new Exception("Silinecek katılımcı bulunamadı.");

            mainDbContext.Participants.Remove(participant);
            mainDbContext.SaveChanges();
        }

    }
}
