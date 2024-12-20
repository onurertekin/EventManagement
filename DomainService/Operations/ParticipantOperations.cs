﻿using DatabaseModel;
using DatabaseModel.Entities;
using DomainService.Base;

namespace DomainService.Operations
{
    public class ParticipantOperations : DbContextHelper
    {
        private MainDbContext mainDbContext;
        public ParticipantOperations(MainDbContext mainDbContext) : base(mainDbContext)
        {
            this.mainDbContext = mainDbContext;
        }

        public IList<Participant> Search(string firstName, string lastName, string email, string phoneNumber, DateTime registeredDate, string password)
        {
            var query = mainDbContext.Participants.AsQueryable();

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

        public void Create(string firstName, string lastName, string email, string phoneNumber, DateTime registeredDate, string password)
        {
            #region Validations

            var currentlyEmail = mainDbContext.Participants.Where(x => x.Email == email).SingleOrDefault();
            if (currentlyEmail != null)
                throw new Exception("Böyle bir email zaten kayıtlı.");

            var currentlyPhoneNumber = mainDbContext.Participants.Where(x => x.PhoneNumber == phoneNumber).SingleOrDefault();
            if (currentlyPhoneNumber != null)
                throw new Exception("Böyle bir telefon numarası zaten kayıtlı");

            #endregion

            var participant = new Participant();
            participant.FirstName = firstName;
            participant.LastName = lastName;
            participant.Email = email;
            participant.PhoneNumber = phoneNumber;
            participant.RegisteredDate = registeredDate;
            participant.Password = password;

            SaveEntity(participant);
        }

        public void Update(int id, string firstName, string lastName, string email, string phoneNumber, DateTime registeredDate, string password)
        {
            #region Validations

            var participant = mainDbContext.Participants.Where(x => x.Id == id).SingleOrDefault();
            if (participant == null)
                throw new Exception("Güncellenicek bir katılımcı bulunamadı.");

            var currentlyEmail = mainDbContext.Participants.Where(x => x.Email == email).SingleOrDefault();
            if (currentlyEmail != null)
                throw new Exception("Böyle bir email zaten kayıtlı.");

            var currentlyPhoneNumber = mainDbContext.Participants.Where(x => x.PhoneNumber == phoneNumber).SingleOrDefault();
            if (currentlyPhoneNumber != null)
                throw new Exception("Böyle bir telefon numarası zaten kayıtlı");

            #endregion

            participant.FirstName = firstName;
            participant.LastName = lastName;
            participant.Email = email;
            participant.PhoneNumber = phoneNumber;
            participant.RegisteredDate = registeredDate;
            participant.Password = password;

            UpdateEntity(participant);

        }

        public void Delete(int id)
        {
            var participant = mainDbContext.Participants.Where(x => x.Id == id).SingleOrDefault();
            if (participant == null)
                throw new Exception("Silinecek katılımcı bulunamadı.");

            DeleteEntity(participant);
        }

    }
}
