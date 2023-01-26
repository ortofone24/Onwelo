using Onwelo.Api.Data;
using System.ComponentModel.DataAnnotations;

namespace Onwelo.Api.Features.Models
{
    using static Validation.Person;
    public class HasVotedPerson
    {
        [Required]
        [MaxLength(MaxDescriptionLength)]
        public string Name { get; set; }

        public bool Voted { get; set; } = true;
    }
}
