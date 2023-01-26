using Onwelo.Api.Data;
using System.ComponentModel.DataAnnotations;

namespace Onwelo.Api.Features.Models
{
    public class CandidatePerson
    {
        public string Name { get; set; }

        public int Votes { get; set; }
    }
}
