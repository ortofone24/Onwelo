using Microsoft.AspNetCore.Mvc;
using Onwelo.Api.Data.Entities;
using Onwelo.Api.Features.Models;
using static Onwelo.Api.Data.Validation;

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

        [Route(nameof(GetAllVotersPersons))]
        [HttpGet]
        public async Task<IActionResult> GetAllVotersPersons()
        {
            var result = await this.voteService.GetAllVotersPersons();
            return Ok(result);
        }

        [Route(nameof(GetAllVotersPersonsWhoAlreadyVoted))]
        [HttpGet]
        public async Task<IActionResult> GetAllVotersPersonsWhoAlreadyVoted()
        {
            var result = await this.voteService.GetAllVotersPersonsWhoAlreadyVoted();
            return Ok(result);
        }

        [Route(nameof(GetAllVotersPersonsWhoNotVoted))]
        [HttpGet]
        public async Task<IActionResult> GetAllVotersPersonsWhoNotVoted()
        {
            var result = await this.voteService.GetAllVotersPersonsWhoNotVoted();
            return Ok(result);
        }

        [Route(nameof(GetAllCandidatesPersons))]
        [HttpGet]
        public async Task<IActionResult> GetAllCandidatesPersons()
        {
            var result = await this.voteService.GetAllCandidatesPersons();
            return Ok(result);
        }


        [Route(nameof(GetCandidateAmountVotes))]
        [HttpGet]
        public async Task<IActionResult> GetCandidateAmountVotes(string name)
        {
            var candidateId = await this.voteService.GetIdFromCandidateByName(name);

            if (candidateId == 0)
            {
                var message = $"The person {name} doesn't exist";

                return BadRequest(message);
            }

            var candidateAmountVotes = await this.voteService.GetCandidateAmountVotes(candidateId, name);

            return Ok(candidateAmountVotes);
        }

        [Route(nameof(CreatePerson))]
        [HttpPost]
        public async Task<IActionResult> CreatePerson([FromBody] CreatePerson person)
        {
            if (await this.voteService.CheckIfNameExists(person.Name))
            {
                var message = $"The person {person.Name} already exists";

                return BadRequest(message);
            }

            var craetedPerson = await this.voteService.CreatePerson(person);

            return Ok(craetedPerson);
        }

        [Route(nameof(IncrementVotesByOne))]
        [HttpPost]
        public async Task<IActionResult> IncrementVotesByOne(string name)
        {
            var candidateId = await this.voteService.GetIdFromCandidateByName(name);

            if (candidateId == 0)
            {
                var message = $"The person {name} doesn't exist";

                return BadRequest(message);
            }

            await this.voteService.IncrementVotesByOne(candidateId);

            return Ok("Voted for candidate");
        }

        [Route(nameof(VoterHasVoted))]
        [HttpPut]
        public async Task<IActionResult> VoterHasVoted([FromBody] HasVotedPerson person)
        {
            await this.voteService.VoterHasVoted(person);

            return Ok("Voted!");
        }
    }
}
