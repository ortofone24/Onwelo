using Microsoft.AspNetCore.Mvc;
using Onwelo.Api.Data.Entities;
using Onwelo.Api.Features.Models;

namespace Onwelo.Api.Features.Votes
{
    public class VoteController : ApiController
    {
        private readonly IVoteService voteService;

        public VoteController(IVoteService voteService)
        {
            this.voteService = voteService;
        }

        [Route(nameof(GetAllPersons))]
        [HttpGet]
        public async Task<IActionResult> GetAllPersons()
        {
            var result = await this.voteService.GetAllPersons();
            return Ok(result);
        }

        [Route(nameof(CreatePerson))]
        [HttpPost]
        public async Task<IActionResult> CreatePerson([FromBody] CreatePerson person)
        {
            var craetedPerson = await this.voteService.CreatePerson(person);

            return Ok(craetedPerson);
        }




        [Route(nameof(GetPersonId))]
        [HttpGet]
        public async Task<int> GetPersonId(string name)
        {

            var tt = await this.voteService.CheckIfNameExists(name);

            if (tt == true)
            {
                var test = "MArcin";
            }

             var result = await this.voteService.GetPersonIdByName(name);
             //if result 0;
             return result;
        }
    }
}
