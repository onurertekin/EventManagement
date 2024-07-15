using Contract.Request.Events;
using Contract.Response.Events;
using DomainService.Operations;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [ApiController]
    [Route("event-management-api/events")]
    public class EventsController : ControllerBase
    {
        private readonly EventOperations eventOperations;

        public EventsController(EventOperations eventOperations)
        {
            this.eventOperations = eventOperations;
        }

        [HttpGet]
        public ActionResult<SearchEventsResponse> Search([FromQuery] SearchEventsRequest request)
        {
            var events = eventOperations.Search(request.organizerId, request.name, request.description, request.location, request.startDate, request.endDate, request.createdOn, request.updatedOn);
            var response = new SearchEventsResponse();

            foreach (var _event in events)
            {
                response.events.Add(new SearchEventsResponse.Event()
                {
                    id = _event.Id,
                    organizerId = _event.OrganizerId,
                    description = _event.Description,
                    name = _event.Name,
                    location = _event.Location,
                    startDate = _event.StartDate,
                    endDate = _event.EndDate,
                    createdOn = _event.CreatedOn,
                    updatedOn = _event.UpdatedOn,

                });
            }

            return new JsonResult(response);
        }

        [HttpGet("{id}")]
        public ActionResult<GetSingleEventResponse> GetSingle(int id)
        {
            var _event = eventOperations.GetSingle(id);
            var response = new GetSingleEventResponse();

            response.id = id;
            response.organizerId = _event.OrganizerId;
            response.name = _event.Name;
            response.description = _event.Description;
            response.location = _event.Location;
            response.startDate = _event.StartDate;
            response.endDate = _event.EndDate;
            response.createdOn = _event.CreatedOn;
            response.updatedOn = _event.UpdatedOn;

            return response;
        }

        [HttpPost]
        public void Create([FromBody] CreateEventRequest request)
        {
            eventOperations.Create(request.organizerId, request.name, request.description, request.location, request.startDate, request.endDate);
        }

        [HttpPut("{id}")]
        public void Update(int id, [FromBody] UpdateEventRequest request)
        {
            eventOperations.Update(id, request.organizerId, request.name, request.description, request.location, request.startDate, request.endDate);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            eventOperations.Delete(id);
        }
    }
}
