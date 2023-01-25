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

                if (nameFromDb.ToString() != string.Empty)
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

                //TODO if is voter is false that's mean is candidate
                if (person.IsVoter == false)
                {
                    try
                    {
                         CreateCandidate(id);
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

        private async void CreateCandidate(int id)
        {
            var param = new DynamicParameters();
            param.Add("@Id", id);

            var query = "INSERT INTO Candidate(PersonId) VALUES (@Id) ";

            using (var connection = this.context.CreateConnection())
            {
                await connection.ExecuteAsync(query, param);
            }
        }
    }
}