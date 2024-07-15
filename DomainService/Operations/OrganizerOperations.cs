using DatabaseModel;
using DatabaseModel.Entities;
using DomainService.Exceptions;

namespace DomainService.Operations
{
    public class OrganizerOperations
    {
        private readonly MainDbContext mainDbContext;

        public OrganizerOperations(MainDbContext mainDbContext)
        {
            this.mainDbContext = mainDbContext;
        }

        public IList<Organizer> Search(string firstName, string lastName, string email, string phoneNumber, string organizerName,string password)
        {
            var query = mainDbContext.Organizers.AsQueryable();

            if (!string.IsNullOrEmpty(firstName))
                query = mainDbContext.Organizers.Where(x => x.FirstName == firstName);
            if (!string.IsNullOrEmpty(lastName))
                query = mainDbContext.Organizers.Where(x => x.LastName == lastName);
            if (!string.IsNullOrEmpty(email))
                query = mainDbContext.Organizers.Where(x => x.Email == email);
            if (!string.IsNullOrEmpty(phoneNumber))
                query = mainDbContext.Organizers.Where(x => x.PhoneNumber == phoneNumber);
            if (!string.IsNullOrEmpty(organizerName))
                query = mainDbContext.Organizers.Where(x => x.OrganizerName == organizerName);
            if (!string.IsNullOrEmpty(password))
                query = mainDbContext.Organizers.Where(x => x.Password == password);

            return query.ToList();

        }

        public Organizer GetSingle(int id)
        {
            var organizer = mainDbContext.Organizers.Where(x => x.Id == id).SingleOrDefault();
            if (organizer == null)
                throw new BusinessException(404, "Organizer bulunamadı.");

            return organizer;
        }

        public void Create(string firstName, string lastName, string email, string phoneNumber, string organizerName,string password)
        {
            #region Validations

            var currentlyEmail = mainDbContext.Organizers.Where(x => x.Email == email).SingleOrDefault();
            if (currentlyEmail != null)
                throw new BusinessException(404,"Böyle bir email zaten mevcut.");

            var currentlyPhoneNumber = mainDbContext.Organizers.Where(x => x.PhoneNumber == phoneNumber).SingleOrDefault();
            if (currentlyPhoneNumber != null)
                throw new BusinessException(404,"Böyle bir telefon numarası zaten mevcut");

            #endregion

            Organizer organizer = new Organizer();
            organizer.FirstName = firstName;
            organizer.LastName = lastName;
            organizer.Email = email;
            organizer.PhoneNumber = phoneNumber;
            organizer.OrganizerName = organizerName;
            organizer.Password = password;

            mainDbContext.Organizers.Add(organizer);
            mainDbContext.SaveChanges();
        }

        public void Update(int id, string firstName, string lastName, string email, string phoneNumber, string organizerName,string password)
        {
            #region Validations

            var organizer = mainDbContext.Organizers.Where(x => x.Id == id).SingleOrDefault();
            if (organizer == null)
                throw new BusinessException(404,"Güncellenicek kayıt bulunamadı.");

            #endregion

            organizer.FirstName = firstName;
            organizer.LastName = lastName;
            organizer.Email = email;
            organizer.PhoneNumber = phoneNumber;
            organizer.OrganizerName = organizerName;
            organizer.Password = password;

            mainDbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var organizer = mainDbContext.Organizers.Where(x => x.Id == id).SingleOrDefault();
            if (organizer == null)
                throw new BusinessException(404,"Silinecek kayıt bulunamadı.");

            mainDbContext.Organizers.Remove(organizer);
            mainDbContext.SaveChanges();
        }
    }
}
