using System.ComponentModel.DataAnnotations;

namespace Onwelo.Api.Data.Entities
{
    using static Validation.Person;

    public class Person
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(MaxDescriptionLength)]
        public string Name { get; set; }
        [Required]
        public bool IsVoter { get; set; }
        public bool Voted { get; set; } = false;
    }
}
