using DatabaseModel;
using DatabaseModel.Entities;
using DomainService.Exceptions;
using DomainService.Base;
using Microsoft.EntityFrameworkCore;

namespace DomainService.Operations
{
    public class EventOperations : DbContextHelper
    {
        private readonly MainDbContext mainDbContext;
        private readonly EMailOperations emailOperations;
        public EventOperations(MainDbContext mainDbContext, EMailOperations emailOperations) : base(mainDbContext)
        {
            this.mainDbContext = mainDbContext;
            this.emailOperations = emailOperations;
        }

        public IList<Event> Search(int? organizerId, string name, string description, string location, DateTime? startDate, DateTime? endDate)
        {
            var query = mainDbContext.Events.AsQueryable();

            if (organizerId.HasValue)
                query = mainDbContext.Events.Where(x => x.OrganizerId == organizerId);

            if (!string.IsNullOrEmpty(name))
                query = mainDbContext.Events.Where(x => x.Name == name);

            if (!string.IsNullOrEmpty(description))
                query = mainDbContext.Events.Where(x => x.Description == description);

            if (!string.IsNullOrEmpty(location))
                query = mainDbContext.Events.Where(x => x.Location == location);

            if (startDate.HasValue)
                query = mainDbContext.Events.Where(x => x.StartDate == startDate);

            if (endDate.HasValue)
                query = mainDbContext.Events.Where(x => x.EndDate == endDate);

            return query.ToList();
        }

        public Event GetSingle(int id)
        {
            var _event = mainDbContext.Events.Where(x => x.Id == id).SingleOrDefault();
            if (_event == null)
                throw new BusinessException(404, "Event bulunamadı.");

            return _event;
        }

        public void Create(int organizerId, string name, string description, string location, DateTime startDate, DateTime endDate)
        {
            #region Validations
            var currentlyOrganizer = mainDbContext.Organizers.Where(x => x.Id == organizerId).SingleOrDefault();
            if (currentlyOrganizer == null)
                throw new BusinessException(404, "Böyle bir organizatör bulunamadı.");
            #endregion

            Event _event = new Event();
            _event.OrganizerId = organizerId;
            _event.Name = name;
            _event.Description = description;
            _event.Location = location;
            _event.StartDate = startDate;
            _event.EndDate = endDate;
            _event.CreatedOn = DateTime.Now;

            SaveEntity(_event);
        }

        public void Update(int id, int organizerId, string name, string description, string location, DateTime startDate, DateTime endDate)
        {
            #region Validations

            var _event = mainDbContext.Events.Where(x => x.Id == id).SingleOrDefault();
            if (_event == null)
                throw new BusinessException(404, "Güncellenecek kayıt bulunamadı.");

            #endregion
            _event.OrganizerId = organizerId;
            _event.Name = name;
            _event.Description = description;
            _event.Location = location;
            _event.StartDate = startDate;
            _event.EndDate = endDate;
            _event.UpdatedOn = DateTime.Now;

            UpdateEntity(_event);
        }

        public void Delete(int id)
        {
            #region Validations

            var _event = mainDbContext.Events.Where(x => x.Id == id).SingleOrDefault();
            if (_event == null)
                throw new BusinessException(404, "Silincek kayıt bulunamadı.");

            if (!_event.IsCancelled)
                throw new BusinessException(404, "Sadece iptal edilecek eventler silinebilir.");

            #endregion

            DeleteEntity(_event);
        }

        public async Task Cancel(int id)
        {
            #region Validations

            var @event = mainDbContext.Events.Include(x => x.Participants).Where(x => x.Id == id).SingleOrDefault();
            if (@event == null)
                throw new BusinessException(404, "Silincek kayıt bulunamadı.");

            if (@event.IsCancelled)
                throw new BusinessException(404, "Bu event zaten iptal edilmiş.");

            #endregion

            @event.IsCancelled = true;
            UpdateEntity(@event);

            #region Send Cancellation EMail to participants

            string subject = $"ETKİNLİK İPTAL: {@event.Name}";
            string body = $"Sevgili #ParticipantName#, <br/><br/> Etkinlik iptal edilmiştir.. #EventName# etkinliğinizin başlangıç tarihi : #EventStartDate#.<br/><br/> <br/>Event Management Team ";

            foreach (var participant in @event.Participants)
            {
                string messageBody = body.Replace("#ParticipantName#", participant.FirstName + " " + participant.LastName);
                messageBody = messageBody.Replace("#EventName#", @event.Name);
                messageBody = messageBody.Replace("#EventStartDate#", @event.StartDate.ToLongDateString());
                await emailOperations.SendEmailAsync(participant.Email, subject, messageBody);
            }

            #endregion
        }

        public async Task Join(int id, int participantId)
        {
            var @event = mainDbContext.Events.Where(x => x.Id == id).SingleOrDefault();
            if (@event == null)
                throw new BusinessException(404, "Event Bulunamadı");

            var participant = mainDbContext.Participants.Where(x => x.Id == id).SingleOrDefault();
            if (participant == null)
                throw new BusinessException(404, "Participant Bulunamadı");

            var alreadyJoined = @event.Participants.Any(p => p.Id == participantId);
            if (alreadyJoined)
                throw new BusinessException(404, "Bu evente zaten katılmışsınız");

            @event.Participants.Add(participant);
            UpdateEntity(@event);
        }
    }
}
