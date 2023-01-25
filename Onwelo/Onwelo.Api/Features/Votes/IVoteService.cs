using Onwelo.Api.Data;
using Onwelo.Api.Data.Entities;
using Onwelo.Api.Features.Models;

namespace Onwelo.Api.Features.Votes
{
    public interface IVoteService
    {
        Task<IEnumerable<Person>> GetAllPersons();
        Task<int> GetPersonIdByName(string name);
        Task<bool> CheckIfNameExists(string name);
        Task<Person> CreatePerson(CreatePerson person);
    }
}
