using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using Onwelo.Api.Data;
using Onwelo.Api.Data.Entities;
using Onwelo.Api.Features.Models;

namespace Onwelo.Api.Features.Votes
{
    public class VoteService : IVoteService
    {
        private readonly DapperContext context;

        public VoteService(DapperContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Person>> GetAllPersons()
        {
            var query = "SELECT * FROM Person";

            using (var connection = this.context.CreateConnection())
            {
                var person = await connection.QueryAsync<Person>(query);

                return person.ToList();
            }
        }

        public async Task<IEnumerable<VoterPerson>> GetAllVotersPersons()
        {
            var query = "SELECT Name, Voted FROM Person WHERE IsVoter = 1";

            using (var connection = this.context.CreateConnection())
            {
                var person = await connection.QueryAsync<VoterPerson>(query);

                return person.ToList();
            }
        }

        public async Task<IEnumerable<VoterPerson>> GetAllVotersPersonsWhoAlreadyVoted()
        {
            var query = "SELECT * FROM Person WHERE Voted = 1 and IsVoter = 1";

            using (var connection = this.context.CreateConnection())
            {
                var person = await connection.QueryAsync<VoterPerson>(query);

                return person.ToList();
            }
        }

        public async Task<IEnumerable<VoterPerson>> GetAllVotersPersonsWhoNotVoted()
        {
            var query = "SELECT * FROM Person WHERE Voted = 0 and IsVoter = 1";

            using (var connection = this.context.CreateConnection())
            {
                var person = await connection.QueryAsync<VoterPerson>(query);

                return person.ToList();
            }
        }

        public async Task<IEnumerable<CandidatePerson>> GetAllCandidatesPersons()
        {
            var query = "SELECT Name, Votes FROM Person p RIGHT JOIN Candidate c " +
                        "on p.Id = c.PersonId";

            using (var connection = this.context.CreateConnection())
            {
                var person = await connection.QueryAsync<CandidatePerson>(query);

                return person.ToList();
            }
        }

        public async Task<int> GetPersonIdByName(string name)
        {
            var param = new DynamicParameters();
            param.Add("@Name", name, System.Data.DbType.String);

            var query = "SELECT Id FROM Person " +
                        "WHERE Name = @Name";

            using (var connection = this.context.CreateConnection())
            {
                var id = await connection.QueryAsync<int>(query, param);
                return id.FirstOrDefault();
            }
        }

        public async Task<bool> CheckIfNameExists(string name)
        {
            var param = new DynamicParameters();
            param.Add("@Name", name, System.Data.DbType.String);

            var query = "SELECT top(1) Name FROM Person " +
                        "WHERE Name = @Name";

            using (var connection = this.context.CreateConnection())
            {
                var nameFromDb = await connection.QueryAsync<string>(query, param);

                if (nameFromDb.Count() != 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<Person> CreatePerson(CreatePerson person)
        {
            var param = new DynamicParameters();
            param.Add("@Name", person.Name, System.Data.DbType.String);
            param.Add("@IsVoter", person.IsVoter , System.Data.DbType.Boolean);

            var query = "INSERT INTO Person(Name, IsVoter) VALUES (@Name, @IsVoter) " +
                        "SELECT CAST(SCOPE_IDENTITY() as int)";

            using (var connection = this.context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, param);

                if (person.IsVoter == false)
                {
                    try
                    {
                         await CreateCandidate(id);
                    }
                    catch (SqlException ex)
                    {
                        //todo log it
                        throw  ;
                    }
                }

                var createdPerson = new Person
                {
                    Id = id,
                    Name = person.Name,
                    IsVoter = person.IsVoter
                };
                return createdPerson;
            }
        }

        public async Task VoterHasVoted(HasVotedPerson voter)
        {
            voter.Voted = true;
            
            var param = new DynamicParameters();
            param.Add("@Name", voter.Name, System.Data.DbType.String);
            param.Add("@Voted", voter.Voted, System.Data.DbType.Boolean);

            var query = "UPDATE Person SET Voted = @Voted " +
                        "WHERE NAME = @Name AND IsVoter = 1";

            using (var connection = this.context.CreateConnection())
            {
                await connection.ExecuteAsync(query, param);
            }
        }

        public async Task IncrementVotesByOne(int candidateId)
        {
            var param = new DynamicParameters();
            param.Add("@PersonId", candidateId, DbType.Int32);

            var query = "UPDATE Candidate SET Votes = Votes + 1 " +
                        "WHERE PersonId = @PersonId";

            using (var connection = this.context.CreateConnection())
            {
                await connection.ExecuteAsync(query, param);
            }
        }

        public async Task<int> GetIdFromCandidateByName(string name)
        {
            var param = new DynamicParameters();
            param.Add("@Name", name, System.Data.DbType.String);

            var query = "SELECT Id FROM Person " +
                        "WHERE Name = @Name AND IsVoter = 0";

            using (var connection = this.context.CreateConnection())
            {
                var id = await connection.QueryAsync<int>(query, param);
                return id.FirstOrDefault();
            }
        }

        public async Task<CandidateAmountVotes> GetCandidateAmountVotes(int id, string name)
        {
            var param = new DynamicParameters();
            param.Add("@Id", id, System.Data.DbType.Int32);

            var query = "SELECT Votes FROM Candidate WHERE PersonId = @Id";

            using (var connection = this.context.CreateConnection())
            {
                var amountVotes = await connection.QueryAsync<int>(query, param);

                var candidateVoteAmounts = new CandidateAmountVotes
                {
                    Name = name,
                    AmountVotes = amountVotes.FirstOrDefault()
                };

                return candidateVoteAmounts;
            }
        }

        #region Helpers Functions
        private async Task CreateCandidate(int id)
        {
            var param = new DynamicParameters();
            param.Add("@Id", id);

            var query = "INSERT INTO Candidate(PersonId) VALUES (@Id) ";

            using (var connection = this.context.CreateConnection())
            {
                await connection.ExecuteAsync(query, param);
            }
        }
        #endregion
    }
}