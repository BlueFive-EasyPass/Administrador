
using Adm.Interface;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adm.Domain
{
    public class Administrador : IAdministradorDTO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public string? Nome { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [PasswordPropertyText]
        public string? Senha { get; set; }
        public int? Level { get; set; }
    }

    public class AdministradorLogin : IAdministradorDTOLogin
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email não pode ser nulo!")]
        public required string Email { get; set; }
        [PasswordPropertyText]
        [Required(ErrorMessage = "Senha não pode ser nula!")]
        public required string Senha { get; set; }
    }
}
