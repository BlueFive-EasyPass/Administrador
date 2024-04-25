
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adm.Interface
{
    public interface IAdministradorDTO
    {
        int? Id { get; set; }
        string? Nome { get; set; }
        [EmailAddress]
        string? Email { get; set; }
        string? Senha { get; set; }
        int? Level { get; set; }
    }
    public interface IAdministradorDTOLogin
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email não pode ser nulo!")]
        string Email { get; set; }
        [PasswordPropertyText]
        [Required(ErrorMessage = "Senha não pode ser nula!")]
        string Senha { get; set; }
    }
}