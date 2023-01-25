using Onwelo.Api.Data;
using System.ComponentModel.DataAnnotations;

namespace Onwelo.Api.Features.Models
{
    using static Validation.Person;

    public class CreatePerson
    {
        [Required]
        [MaxLength(MaxDescriptionLength)]
        public string Name { get; set; }
        [Required]
        public bool IsVoter { get; set; }
        public bool Voted { get; set; } = false;
    }
}
