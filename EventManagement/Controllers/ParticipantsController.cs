﻿using Contract.Request.Participant;
using Contract.Response.Participant;
using DomainService.Operations;
using Host.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [ApiController]
    [Route("event-management-api/participants")]
    public class ParticipantsController : ControllerBase
    {
        private readonly ParticipantOperations participantOperations;
        public ParticipantsController(ParticipantOperations participantOperations)
        {
            this.participantOperations = participantOperations;
        }

        [HttpGet]
        [Authorizable]
        public ActionResult<SearchParticipantResponse> Search([FromQuery] SearchParticipantRequest request)
        {
            var participants = participantOperations.Search(request.firstName, request.lastName, request.email, request.phoneNumber, request.registeredDate, request.password);

            var response = new SearchParticipantResponse();
            foreach (var participant in participants)
            {
                response.participants.Add(new SearchParticipantResponse.Participants()
                {
                    id = participant.Id,
                    firstName = participant.FirstName,
                    lastName = participant.LastName,
                    email = participant.Email,
                    phoneNumber = participant.PhoneNumber,
                    registeredDate = participant.RegisteredDate,
                    password = participant.Password
                });
            }

            return new JsonResult(response);
        }

        [HttpGet("{id}")]
        [Authorizable]
        public ActionResult<GetSingleParticipantResponse> GetSingle(int id)
        {
            var participant = participantOperations.GetSingle(id);

            var response = new GetSingleParticipantResponse();
            response.id = participant.Id;
            response.firstName = participant.FirstName;
            response.lastName = participant.LastName;
            response.email = participant.Email;
            response.phoneNumber = participant.PhoneNumber;
            response.registeredDate = participant.RegisteredDate;
            response.password = participant.Password;

            return response;

        }

        [HttpPost]
        [Authorizable]
        public void Create([FromBody] CreateParticipantRequest request)
        {
            participantOperations.Create(request.firstName, request.lastName, request.email, request.phoneNumber, request.registeredDate, request.password);
        }

        [HttpPut("{id}")]
        [Authorizable]
        public void Update([FromBody] UpdateParticipantRequest request, int id)
        {
            participantOperations.Update(id, request.firstName, request.lastName, request.email, request.phoneNumber, request.registeredDate, request.password);
        }

        [HttpDelete("{id}")]
        [Authorizable]
        public void Delete(int id)
        {
            participantOperations.Delete(id);
        }
    }
}
