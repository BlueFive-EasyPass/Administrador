
namespace Adm.Interface
{
    public interface IAuth
    {
        string CreateToken(IAdministradorDTO administrador);
    }
}