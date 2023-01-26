using Onwelo.Api.Data;
using Onwelo.Api.Data.Entities;
using Onwelo.Api.Features.Models;

namespace Onwelo.Api.Features.Votes
{
    public interface IVoteService
    {
        Task<IEnumerable<Person>> GetAllPersons();
        Task<IEnumerable<VoterPerson>> GetAllVotersPersons();
        Task<IEnumerable<VoterPerson>> GetAllVotersPersonsWhoAlreadyVoted();
        Task<IEnumerable<VoterPerson>> GetAllVotersPersonsWhoNotVoted();
        Task<IEnumerable<CandidatePerson>> GetAllCandidatesPersons();
        Task<int> GetPersonIdByName(string name);
        Task<bool> CheckIfNameExists(string name);
        Task<Person> CreatePerson(CreatePerson person);
        Task VoterHasVoted(HasVotedPerson voter);
        Task IncrementVotesByOne(int candidateId);
        Task<int> GetIdFromCandidateByName(string name);
        Task<CandidateAmountVotes> GetCandidateAmountVotes(int id, string name);
    }
}
