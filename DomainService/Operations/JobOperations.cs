using DatabaseModel;
using Microsoft.EntityFrameworkCore;

namespace DomainService.Operations
{
    public class JobOperations
    {
        private readonly MainDbContext mainDbContext;
        private readonly EMailOperations emailOperations;

        public JobOperations(MainDbContext mainDbContext, EMailOperations emailOperations)
        {
            this.mainDbContext = mainDbContext;
            this.emailOperations = emailOperations;
        }

        public async Task ExecuteAsync()
        {
            var upcomingEvents = await mainDbContext.Events.Include(x => x.Participants)
                .Where(e => e.StartDate > DateTime.Now && e.StartDate <= DateTime.Now.AddDays(15))
                .ToListAsync();

            string body = $"Sevgili #ParticipantName#, <br/><br/> Etkinlik tarihiniz yaklaşıyor. #EventName# etkinliğinizin başlangıç tarihi : #EventStartDate#.<br/><br/> <br/>Event Management Team ";

            foreach (var @event in upcomingEvents)
            {
                string subject = $"ETKİNLİK: {@event.Name}";

                foreach (var participant in @event.Participants)
                {
                    string messageBody = body.Replace("#ParticipantName#", participant.FirstName + " " + participant.LastName);
                    messageBody = messageBody.Replace("#EventName#", @event.Name);
                    messageBody = messageBody.Replace("#EventStartDate#", @event.StartDate.ToLongDateString());
                    await emailOperations.SendEmailAsync(participant.Email, subject, messageBody);
                }
            }
        }
    }
}
