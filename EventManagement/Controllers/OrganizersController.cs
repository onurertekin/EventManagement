using Contract.Request.Organizers;
using Contract.Response.Organizers;
using DomainService.Operations;
using Host.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [ApiController]
    [Route("event-management-api/organizers")]
    public class OrganizersController : ControllerBase
    {
        private readonly OrganizerOperations organizerOperations;

        public OrganizersController(OrganizerOperations organizerOperations)
        {
            this.organizerOperations = organizerOperations;
        }


        [HttpGet]
        [Authorizable]
        public ActionResult<SearchOrganizersResponse> Search([FromQuery] SearchOrganizersRequest request)
        {
            var organizers = organizerOperations.Search(request.firstName, request.lastName, request.email, request.phoneNumber, request.organizerName, request.password);
            SearchOrganizersResponse response = new SearchOrganizersResponse();
            foreach (var organizer in organizers)
            {
                response.organizers.Add(new SearchOrganizersResponse.Organizers()
                {
                    id = organizer.Id,
                    firstName = organizer.FirstName,
                    lastName = organizer.LastName,
                    email = organizer.Email,
                    phoneNumber = organizer.PhoneNumber,
                    organizerName = organizer.OrganizerName,
                    password = organizer.Password,
                });
            }

            return new JsonResult(response);
        }

        [HttpGet("{id}")]
        [Authorizable]
        public ActionResult<GetSingleOrganizerResponse> GetSingle(int id)
        {
            var organizer = organizerOperations.GetSingle(id);
            GetSingleOrganizerResponse response = new GetSingleOrganizerResponse();

            response.id = id;
            response.phoneNumber = organizer.PhoneNumber;
            response.firstName = organizer.FirstName;
            response.lastName = organizer.LastName;
            response.email = organizer.Email;
            response.organizerName = organizer.OrganizerName;
            response.password = organizer.Password;

            return response;
        }

        [HttpPost]
        [Authorizable]
        public void Create([FromBody] CreateOrganizerRequest request)
        {
            organizerOperations.Create(request.firstName, request.lastName, request.email, request.phoneNumber, request.organizerName, request.password);
        }

        [HttpPut("{id}")]
        [Authorizable]
        public void Update([FromBody] UpdateOrganizerRequest request, int id)
        {
            organizerOperations.Update(id, request.firstName, request.lastName, request.email, request.phoneNumber, request.organizerName, request.password);
        }

        [HttpDelete("{id}")]
        [Authorizable]
        public void Delete(int id)
        {
            organizerOperations.Delete(id);
        }
    }
}
