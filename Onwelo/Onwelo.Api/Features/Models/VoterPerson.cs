using Onwelo.Api.Data;
using System.ComponentModel.DataAnnotations;

namespace Onwelo.Api.Features.Models
{
    public class VoterPerson
    {
        public string Name { get; set; }

        public  bool Voted { get; set; }
    }
}
