using DatabaseModel;
using DatabaseModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Operations
{
    public class EventOperations
    {
        private readonly MainDbContext mainDbContext;
        public EventOperations(MainDbContext mainDbContext)
        {
            this.mainDbContext = mainDbContext;
        }

        public IList<Event> Search(int organizerId, string name, string description, string location, DateTime startDate, DateTime endDate, DateTime createdOn, DateTime updatedOn)
        {
            var query = mainDbContext.Events.AsQueryable();
            if (!string.IsNullOrEmpty(name))
                query = mainDbContext.Events.Where(x => x.Name == name);
            if (!string.IsNullOrEmpty(description))
                query = mainDbContext.Events.Where(x => x.Description == description);
            if (!string.IsNullOrEmpty(location))
                query = mainDbContext.Events.Where(x => x.Location == location);
            if (startDate != default(DateTime))
                query = mainDbContext.Events.Where(x => x.StartDate == startDate);
            if (endDate != default(DateTime))
                query = mainDbContext.Events.Where(x => x.EndDate == endDate);
            if (createdOn != default(DateTime))
                query = mainDbContext.Events.Where(x => x.CreatedOn == createdOn);
            if (updatedOn != default(DateTime))
                query = mainDbContext.Events.Where(x => x.UpdatedOn == updatedOn);

            return query.ToList();
        }

        public Event GetSingle(int id)
        {
            var _event = mainDbContext.Events.Where(x => x.Id == id).SingleOrDefault();
            if (_event == null)
                throw new Exception("Event bulunamadı.");

            return _event;
        }

        public void Create(int organizerId, string name, string description, string location, DateTime startDate, DateTime endDate)
        {
            #region Validations
            var currentlyOrganizer = mainDbContext.Organizers.Where(x => x.Id == organizerId).SingleOrDefault();
            if (currentlyOrganizer == null)
                throw new Exception("Böyle bir organizatör bulunamadı.");
            #endregion

            Event _event = new Event();
            _event.OrganizerId = organizerId;
            _event.Name = name;
            _event.Description = description;
            _event.Location = location;
            _event.StartDate = startDate;
            _event.EndDate = endDate;
            _event.CreatedOn = DateTime.Now;

            mainDbContext.Events.Add(_event);
            mainDbContext.SaveChanges();
        }

        public void Update(int id, int organizerId, string name, string description, string location, DateTime startDate, DateTime endDate)
        {
            #region Validations

            var _event = mainDbContext.Events.Where(x => x.Id == id).SingleOrDefault();
            if (_event == null)
                throw new Exception("Güncellenecek kayıt bulunamadı.");

            #endregion
            _event.OrganizerId = organizerId;
            _event.Name = name;
            _event.Description = description;
            _event.Location = location;
            _event.StartDate = startDate;
            _event.EndDate = endDate;
            _event.UpdatedOn = DateTime.Now;

            mainDbContext.SaveChanges();
        }
        public void Delete(int id)
        {
            #region Validations

            var _event = mainDbContext.Events.Where(x => x.Id == id).SingleOrDefault();
            if (_event == null)
                throw new Exception("Silincek kayıt bulunamadı.");

            #endregion

            mainDbContext.Events.Remove(_event);
            mainDbContext.SaveChanges();

        }
    }
}
