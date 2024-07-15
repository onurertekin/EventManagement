using Contract.Request.Participant;
using Contract.Response.Participant;
using DomainService.Operations;
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
        public ActionResult<SearchParticipantResponse> Search([FromQuery] SearchParticipantRequest request)
        {
            var participants = participantOperations.Search(request.eventId, request.firstName, request.lastName, request.email, request.phoneNumber, request.registeredDate, request.password);
            SearchParticipantResponse response = new SearchParticipantResponse();
            foreach (var participant in participants)
            {
                response.participants.Add(new SearchParticipantResponse.Participants()
                {
                    id = participant.Id,
                    eventId = participant.EventId,
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
        public ActionResult<GetSingleParticipantResponse> GetSingle(int id)
        {
            var participant = participantOperations.GetSingle(id);

            var response = new GetSingleParticipantResponse();
            response.eventId = participant.Id;
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
        public void Create([FromBody] CreateParticipantRequest request)
        {
            participantOperations.Create(request.eventId, request.firstName, request.lastName, request.email, request.phoneNumber, request.registeredDate, request.password);
        }

        [HttpPut("{id}")]
        public void Update([FromBody] UpdateParticipantRequest request, int id)
        {
            participantOperations.Update(id, request.eventId, request.firstName, request.lastName, request.email, request.phoneNumber, request.registeredDate, request.password);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            participantOperations.Delete(id);
        }
    }
}
