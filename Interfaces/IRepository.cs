using Adm.Domain;
using Adm.Interface;

namespace Adm.Interface
{
    public interface IRepository
    {
        Task<IResultadoOperacao<List<Administrador>>> GetAdministradoresAsync(IAdministradorDTO query);
        Task<IResultadoOperacao<IAdministradorDTO>> Create(IAdministradorDTO administrador);
        Task<ResultadoOperacao<IAdministradorDTO>> Edit(IAdministradorDTO administrador);
        Task<ResultadoOperacao<IAdministradorDTO>> Delete(IAdministradorDTO administrador);
    }
}