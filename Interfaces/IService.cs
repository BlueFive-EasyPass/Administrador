using Adm.Domain;

namespace Adm.Interface
{
    public interface IService
    {
        Task<IResultadoOperacao<List<Administrador>>> GetAdministradoresAsync(IAdministradorDTO query);
        Task<IResultadoOperacao<IAdministradorDTO>> Create(IAdministradorDTO administrador);

        Task<IResultadoOperacao<IAdministradorDTO>> Edit(IAdministradorDTO administrador);
        Task<IResultadoOperacao<IAdministradorDTO>> Delete(IAdministradorDTO administrador);
        Task<IResultadoOperacao<string>> Login(IAdministradorDTOLogin administrador);
    }
}